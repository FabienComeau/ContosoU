﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ContosoU.Data;
using ContosoU.Models;
using ContosoU.Services;


namespace ContosoU
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //fcomeau: School services this is one of the steps for migrating
            services.AddDbContext<SchoolContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            ////
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            /*fcomeau: ASP.NET services can be configured with the following lifetimes:
             * ===========
             * =transietn=
             * ===========
             * Transient lifetime services are created each time they are requested.
             * this lifetime works for lightweight, stateless services
             * 
             * ===========
             * ==scoped===
             * ===========
             * Scoped lifetime services are created once per request
             * 
             * ===========
             * =singleton=
             * ===========
             * Singleton lifetime services are created the first time they are requested
             * (or when ConfigureServices is run if you specify the instence there) and
             * then every subsequent request will use the same instance
             * 
             */
            //fcomeau: service for seeding admin user and roles
            services.AddTransient<AdministratorSeedData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,SchoolContext context,
            AdministratorSeedData seeder)//fcomeau: add AdminstratorSeedData middleware to pipeline/*fcomeau: add schoolcontext middleware to the pipeline*/
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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

            app.UseStaticFiles();//this allows you to use static file

            app.UseIdentity();//this allows you to use identity framwork (register and logging)

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            //initialize the database with SEED data
            //DbInitializer.Initialize(context);
            /*the first time you run the application the database will be created and seeded with test data.
             * whenever you change your data model, you can delete the database, update
             * your seed method and start fresh with a new
             */

            //seed the Administrator and roles
            await seeder.EnsureSeedDatat();
        }
    }
}
