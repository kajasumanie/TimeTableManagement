using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableManagement.DataContext;
using TimeTableManagement.Models;

namespace TimeTableManagement.Services
{
    public class UserService : IUserService
    {

        private ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }



        public Task<dynamic> SaveLeave(UserModel m)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "dummy").Options;
            using (var context = new ApplicationDbContext(options))
            {
                //var user = new UserModel
                //{
                //    ID = 5,
                //    Role = "Student"
                //};

                context.userTbl.Add(m);
                context.SaveChanges();
            }


            //dynamic myDynamicOBJ = new ExpandoObject();
            return null;

            //throw new NotImplementedException();
        }

        public Task<dynamic> UpdateLeave(UserModel m)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "dummy").Options;
            using (var context = new ApplicationDbContext(options))
            {
                //var user = new UserModel
                //{
                //    ID = 5,
                //    Role = "Student"
                //};

                context.userTbl.Update(m);
                context.SaveChanges();
            }


            //dynamic myDynamicOBJ = new ExpandoObject();
            return null;

            //throw new NotImplementedException();
        }

        public Task<dynamic> GetSingByRole(string role)
        {
            //var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "dummy").Options;
            //using (var context = new ApplicationDbContext(options))
            //{
            //    //var user = new UserModel
            //    //{
            //    //    ID = 5,
            //    //    Role = "Student"
            //    //};

            //    context.userTbl.Update(m);
            //    context.SaveChanges();
            //}
            var userss = _context.userTbl.Where(x => x.Role == role).ToList();

            //dynamic myDynamicOBJ = new ExpandoObject();
            return null;

            //throw new NotImplementedException();
        }
    }
}
