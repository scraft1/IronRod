using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IronRod.Data;
using IronRod.Models;
using IronRod.Services;

namespace IronRod
{
    public class Startup
    {
        private IHostingEnvironment _env;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            _env = env;
            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<PassagesDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ScripturesDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("LdsScripturesConnection"))); 

            services.AddIdentity<ApplicationUser, IdentityRole>(
                config => {
                    config.User.RequireUniqueEmail = true;
                    // config.Password.RequiredLength = 8;
                    // config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                }
            )  
                .AddEntityFrameworkStores<PassagesDbContext>()
                .AddDefaultTokenProviders(); 

            services.AddMvc(config =>
            {
                if(_env.IsProduction()){
                    config.Filters.Add(new RequireHttpsAttribute());
                }
            }
            );

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            // seed data 
            services.AddTransient<PassagesContextSeedData>();

            // implement repository interfaces 
            services.AddScoped<IPassagesRepository, PassagesRepository>();
            services.AddScoped<IScripturesRepository, ScripturesRepository>();

            // Logging
            //services.AddLogging(); // needed ?? 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, PassagesContextSeedData seeder)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(); // parameter LogLevel.Information or Error ?? 

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            seeder.PlantSeedData().Wait(); 
        }
    }
}
