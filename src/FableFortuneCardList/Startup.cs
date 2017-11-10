using FableFortuneCardList.Data;
using FableFortuneCardList.Models;
using FableFortuneCardList.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FableFortuneCardList
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
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();            
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

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

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
                string[] roleNames = { "Admin", "DeckMaster", "User" };
                string[] roleDescs = { "Administrative Role", "Deckmaster Role", "User Role" };
                IdentityResult roleResult;

                for (int x = 0; x < roleNames.Length; x++)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleNames[x]);
                    if (!roleExist)
                    {
                        // create the roles and seed them to the database
                        ApplicationRole newRole = new ApplicationRole();
                        newRole.Name = roleNames[x];
                        newRole.Description = roleDescs[x];
                        newRole.IPAddress = "localhost";
                        roleResult = await roleManager.CreateAsync(newRole);
                    }
                }
            }
        }
    }
}
