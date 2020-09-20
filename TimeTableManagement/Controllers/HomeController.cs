using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TimeTableManagement.DataContext;
using TimeTableManagement.Models;
using TimeTableManagement.Services;

namespace TimeTableManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;
        private IMemoryCache memoryCache;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IMemoryCache memoryCache, IUserService userService)
        {
            _logger = logger;
            _context = context;
            this.memoryCache = memoryCache;
            _userService = userService;
        }

        public IActionResult IndexAsync() { return View(); }

        public IActionResult Privacy() { return View(); }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region this function will save and update user details in the in memory db
        [HttpPost]
        public JsonResult SaveLeave(string role, int isMonday, int isWednesday, int isFriday)
        {
            if(role == "Student")
                return Json("");

            // check if exist
            var user = _context.userTbl.Where(x => x.Role == role).SingleOrDefault();

            // if the user is not found in the in memory db
            // then it means it is new
            // then save it to the memory db
            if (user == null)
            {
                // creating new model.
                var new_user = new UserModel
                {
                    Role = role,
                    LeaveOnMonday = isMonday,
                    LeaveOnWednesday = isWednesday,
                    LeaveOnFriday = isFriday
                };

                // save in the memory db
                _userService.SaveLeave(new_user);
            }
            else
            {
                // else if the user is exist in the memory db
                // update the fields 
                user.LeaveOnMonday = isMonday;
                user.LeaveOnWednesday = isWednesday;
                user.LeaveOnFriday = isFriday;

                // update in the memory db
                _userService.UpdateLeave(user);
            }

            autoShuffle(role);

            return Json("");
        }
        #endregion

        public void autoShuffle(string role)
        {
            // student time table pattern
            // Monday English => Math => Science
            // Wednesday Science => English => Math
            // Friday Science => Math => English

            // check what is the nearest teacher then to be replace
            // in the slot

            // if the teacher has already occupied. then proceed to another teacher

            var user = _context.userTbl.Where(x => x.Role == role).SingleOrDefault();

            if (role == "English Teacher")
            {
                if (user.LeaveOnMonday == 1) { 
                    if(checkIfTeacherIsNotYetOccupied("Math Teacher"))
                        user.SubstituteTeacher = "Math Teacher";
                    else if (checkIfTeacherIsNotYetOccupied("Science Teacher"))
                        user.SubstituteTeacher = "Science Teacher";
                }
                else if (user.LeaveOnWednesday == 1)
                {
                    if (checkIfTeacherIsNotYetOccupied("Science Teacher"))
                        user.SubstituteTeacher = "Science Teacher";
                    else if (checkIfTeacherIsNotYetOccupied("Math Teacher"))
                        user.SubstituteTeacher = "Math Teacher";
                }
                else if (user.LeaveOnFriday == 1)
                {
                    if (checkIfTeacherIsNotYetOccupied(role))
                        user.SubstituteTeacher = "Math Teacher";
                    else if (checkIfTeacherIsNotYetOccupied("Science Teacher"))
                        user.SubstituteTeacher = "Science Teacher";
                }
            }
            else if (role == "Math Teacher")
            {
                if (user.LeaveOnMonday == 1)
                {
                    if (checkIfTeacherIsNotYetOccupied("Science Teacher"))
                        user.SubstituteTeacher = "Science Teacher";
                    else if (checkIfTeacherIsNotYetOccupied("English Teacher"))
                        user.SubstituteTeacher = "English Teacher";
                }
                else if (user.LeaveOnWednesday == 1)
                {
                    if (checkIfTeacherIsNotYetOccupied("English Teacher"))
                        user.SubstituteTeacher = "English Teacher";
                    else if (checkIfTeacherIsNotYetOccupied("Science Teacher"))
                        user.SubstituteTeacher = "Science Teacher";
                }
                else if (user.LeaveOnFriday == 1)
                {
                    if (checkIfTeacherIsNotYetOccupied("English Teacher"))
                        user.SubstituteTeacher = "English Teacher";
                    else if (checkIfTeacherIsNotYetOccupied("Science Teacher"))
                        user.SubstituteTeacher = "Science Teacher";
                }
            }
            else if (role == "Science Teacher")
            {
                if (user.LeaveOnMonday == 1)
                {
                    if (checkIfTeacherIsNotYetOccupied("Math Teacher"))
                        user.SubstituteTeacher = "Math Teacher";
                    else if (checkIfTeacherIsNotYetOccupied("English Teacher"))
                        user.SubstituteTeacher = "English Teacher";
                }
                else if (user.LeaveOnWednesday == 1)
                {
                    if (checkIfTeacherIsNotYetOccupied("English Teacher"))
                        user.SubstituteTeacher = "English Teacher";
                    else if (checkIfTeacherIsNotYetOccupied("Math Teacher"))
                        user.SubstituteTeacher = "Math Teacher";
                }
                else if (user.LeaveOnFriday == 1)
                {
                    if (checkIfTeacherIsNotYetOccupied("Math Teacher"))
                        user.SubstituteTeacher = "Math Teacher";
                    else if (checkIfTeacherIsNotYetOccupied("English Teacher"))
                        user.SubstituteTeacher = "English Teacher";
                }
            }

            // if above not met the condition then 
            // it will notify the sectional head and principal

            _userService.UpdateLeave(user);
        }

        // this function only check if the teacher is no yet assign by another
        public bool checkIfTeacherIsNotYetOccupied(string role)
        {
            bool ret = false;

            var user = _context.userTbl.Where(x => x.Role == role).SingleOrDefault();

            if (user == null || user.SubstituteTeacher == "" || user.SubstituteTeacher == null)
                return true;

            return ret;
        }

        [HttpGet]
        public JsonResult GetUserDetails(string role)
        {
            if (role == "Student")
                return Json("");



            //check if exist
            UserModel users = _context.userTbl.Where(x => x.Role == role).SingleOrDefault();

            if(users == null)
                return Json("");

            //var userss = _context.userTbl.Where(x => x.Role == role).ToList();
            return Json(users);
        }

        [HttpGet]
        public JsonResult CheckIfThereIsAvailableSlot()
        {
            string ret = "clean";
            var user = _context.userTbl.Where(x => x.Role == "English Teacher").SingleOrDefault();
            if(user != null)
            {
                if (user.LeaveOnMonday == 1 || user.LeaveOnWednesday == 1 || user.LeaveOnFriday == 1
                    && (user.SubstituteTeacher == "" || user.SubstituteTeacher == null))
                    ret = "have";
            }

            user = _context.userTbl.Where(x => x.Role == "Science Teacher").SingleOrDefault();
            if (user != null)
            {
                if (user.LeaveOnMonday == 1 || user.LeaveOnWednesday == 1 || user.LeaveOnFriday == 1
                    && (user.SubstituteTeacher == "" || user.SubstituteTeacher == null))
                    ret = "have";
            }

            user = _context.userTbl.Where(x => x.Role == "Math Teacher").SingleOrDefault();
            if (user != null)
            {
                if (user.LeaveOnMonday == 1 || user.LeaveOnWednesday == 1 || user.LeaveOnFriday == 1
                    && (user.SubstituteTeacher == "" || user.SubstituteTeacher == null))
                    ret = "have";
            }

            //var userss = _context.userTbl.Where(x => x.Role == role).ToList();
            return Json(ret);
        }
    }
}
