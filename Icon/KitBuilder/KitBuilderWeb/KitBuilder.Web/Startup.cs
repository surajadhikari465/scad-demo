using KitBuilder.Web.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace KitBuilder.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Load timeout from appSettings, or default to 300.
                var timeoutInSeconds = int.Parse(!string.IsNullOrEmpty(Configuration["Session:TimeoutInSeconds"]) 
                    ? Configuration["Session:TimeoutInSeconds"]
                    : "300");
                options.IdleTimeout = TimeSpan.FromSeconds(timeoutInSeconds);
                options.Cookie.HttpOnly = true;
            });

            services.AddTransient<IKitBuilderUserProfile, KitBuilderUserProfile>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() || env.IsLocal())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();
            app.Use((httpContext, nextMiddleware) =>
            {
                // inject the KitBuilderUserProfile into a session variable if it doesnt exist.
                var kbUser = httpContext.RequestServices.GetRequiredService<IKitBuilderUserProfile>();
                if (httpContext.Session.GetObject<AdUserInformation>("KitBuilderUserProfile") == null)
                {
                    var user = kbUser.GetUserInformation(httpContext.User.Identity.Name);
                    httpContext.Session.SetObject("KitBuilderUserProfile", user);
                }
                return nextMiddleware();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
