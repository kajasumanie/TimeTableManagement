using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using TimeTableManagement.Models;
using IUserService = TimeTableManagement.Services.IUserService;

namespace TimetableTEST
{
    public class Tests
    {
        public IUserService _userService;

        private Mock<IUserService> userService;
        public List<UserModel> users;

        public Tests(IUserService userService)
        {
            this._userService = userService;
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]

        public void TestLeave()
        {

            var users = GetTestUser();
            // users.Add(new UserModel { Role="Student", LeaveOnMonday=1, LeaveOnWednesday =1, LeaveOnFriday =1, CoverDate =""});
            //userService.Setup(a => a.GetAllQueryable()).Returns(users.AsQueryable());
            var result = _userService.SaveLeave(users[0]);
            Assert.IsNotNull(result);
            Assert.AreEqual(null, result);
        }

        private List<UserModel> GetTestUser()
        {
            var testusers = new List<UserModel>();
            testusers.Add(new UserModel { Role = "Student", LeaveOnMonday = 1, LeaveOnWednesday = 1, LeaveOnFriday = 1, SubstituteTeacher = "" });


            return testusers;
        }
    }
}