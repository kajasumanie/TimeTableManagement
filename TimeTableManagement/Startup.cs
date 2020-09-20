using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TimeTableManagement.DataContext;
using TimeTableManagement.Models;
using TimeTableManagement.Services;

namespace TimeTableManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ApplicationDbContext>(options => options.UseMySql("server=localhost;port=3306;userid=root;password=root;database=dummy;persistsecurityinfo=True"));

            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("dummy"));

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "InMemoryDb";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                //options.LoginPath = "Home/Index";
            });

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            //services.AddMvc();
            services.AddMvc(options => options.EnableEndpointRouting = false);

            // services.AddCaching();
            services.AddMemoryCache();

            services.AddControllersWithViews();

            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            //var context = app.ApplicationServices.GetService<ApplicationDbContext>();
            //AddTestData(context);

            app.UseMvc();

        }

        //private static void AddTestData(ApplicationDbContext context)
        //{
        //    var testUser1 = new test
        //    {
        //        ID = 1,
        //        Name = "Luke"
        //    };

        //    context.testtbl.Add(testUser1);

        //    context.SaveChanges();
        //}
    }
}
