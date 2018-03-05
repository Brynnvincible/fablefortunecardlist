using FableFortuneCardList.Data;
using FableFortuneCardList.Models;
using FableFortuneCardList.Services;
using FableFortuneCardList.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace FableFortuneCardList
{
    public class Startup
    {
        private IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Automatically perform database migration
            services.BuildServiceProvider().GetService<ApplicationDbContext>().Database.Migrate();

            services.AddIdentity<ApplicationUser, ApplicationRole>(
                o => {
                    // Allow spaces in Usernames
                    o.User.AllowedUserNameCharacters += " ";
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                if (_env.IsDevelopment())
                {                    
                    options.User.RequireUniqueEmail = false;
                }
                else
                {
                    options.User.RequireUniqueEmail = true;
                }
            });
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie(cookieOptions => cookieOptions.LoginPath = new PathString("/login"))
                        .AddFacebook(facebookOptions =>
                        {
                            facebookOptions.AppId = Environment.GetEnvironmentVariable("Authentication:Facebook:AppId");
                            facebookOptions.AppSecret = Environment.GetEnvironmentVariable("Authentication:Facebook:AppSecret");
                        });

            services.AddMvc();

            // Allow large files to be uploaded
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.Configure<AuthMessageSenderOptions>(o =>
            {
                o.GmailEmailAddress = Environment.GetEnvironmentVariable("Authentication:Gmail:EmailAddress");
                o.GmailEmailPassword = Environment.GetEnvironmentVariable("Authentication:Gmail:Password");
            });                       
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            var serviceProvider = app.ApplicationServices.GetService<IServiceProvider>();

            try
            {
                await CreateRoles(app);
            }
            catch { }
        }

        private async Task CreateRoles(IApplicationBuilder app)
        {
            // adding roles
            IServiceScopeFactory scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                RoleManager<ApplicationRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                IdentityResult roleResult;

                foreach (Roles role in Roles.AllRoles)
                {
                    var roleExist = await roleManager.RoleExistsAsync(role.Name);
                    if (!roleExist)
                    {
                        // create the roles and seed them to the database
                        ApplicationRole newRole = new ApplicationRole
                        {
                            Name = role.Name,
                            Description = role.Description,
                            IPAddress = string.Empty
                        };                        
                        roleResult = await roleManager.CreateAsync(newRole);
                    }
                }
            }
        }
    }
}
