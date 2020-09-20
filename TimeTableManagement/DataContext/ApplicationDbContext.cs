using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableManagement.Models;

namespace TimeTableManagement.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        { }

        public DbSet<test> testtbl { get; set; }
        public DbSet<UserModel> userTbl { get; set; }

        internal object Include(Func<object, object> p)
        {
            throw new NotImplementedException();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseMySql("server=localhost;port=3306;userid=root;password=root;database=dummy;persistsecurityinfo=True")
        //        .UseLoggerFactory(LoggerFactory.Create(b => b
        //        .AddConsole()
        //        .AddFilter(level => level >= LogLevel.Information)))
        //        .EnableSensitiveDataLogging()
        //        .EnableDetailedErrors();
        //}
    }
}
