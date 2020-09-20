using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableManagement.Models;

namespace TimeTableManagement.Services
{
    public interface IUserService
    {
        Task<dynamic> SaveLeave(UserModel m);
        Task<dynamic> UpdateLeave(UserModel m);

        Task<dynamic> GetSingByRole(string role);
    }
}
