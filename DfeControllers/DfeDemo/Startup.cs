using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DfeDemo.Data;
using DfeDemo.Models;
using DfeDemo.Services;

namespace DfeDemo
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IHostingEnvironment env)
		{
			Configuration = configuration;

			var builder = new ConfigurationBuilder();

			if (env.IsDevelopment())
			{
				builder.AddUserSecrets<Startup>();
			}

			Configuration = builder.Build();

		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(Configuration["DefaultConnection"]));

			services.AddDbContext<DataDbContext>(options =>
				options.UseSqlServer(Configuration["DataConnection"]));

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			ConfigureIdentityOptions(services);
			ConfigureApplicationCookie(services);

			// Add application services.
			services.AddTransient<IEmailSender, EmailSender>();

			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			// this bit of code enables authentication on the application
			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}

		private void ConfigureIdentityOptions(IServiceCollection services)
		{
			services.Configure<IdentityOptions>(options =>
			{
				/*
				Enforcing a password policy doesn't improve password security
				Allow your users to use anything on the password and only require
				minimum length. Password hashing takes care of 
				 
				*/
				// Password settings

				//options.Password.RequireDigit = true;
				options.Password.RequiredLength = 8;
				//options.Password.RequireNonAlphanumeric = false;
				//options.Password.RequireUppercase = true;
				//options.Password.RequireLowercase = false;
				//options.Password.RequiredUniqueChars = 6;


				/*
				Configuring a lockout policy can result to a DDoS for your users
				by locking everyone out through account enumeration attacks
				// Lockout settings
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
				options.Lockout.MaxFailedAccessAttempts = 10;
				options.Lockout.AllowedForNewUsers = true;
				*/
				// User settings
				options.User.RequireUniqueEmail = true;
			});
		}

		private void ConfigureApplicationCookie(IServiceCollection services)
		{
			services.ConfigureApplicationCookie(options =>
			{

				/*
				As tempting as it may be, try to avoid using long-expiring cookies as they
				introduce an additional security risk and become an attack vector
				*/

				// Cookie settings
				options.Cookie.HttpOnly = true;
				options.Cookie.Expiration = TimeSpan.FromDays(150);
				options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
				options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
				options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
				options.SlidingExpiration = true;

			});
		}
	}
}
