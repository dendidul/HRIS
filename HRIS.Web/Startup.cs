using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRIS.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRIS.Web
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
            //services.AddDbContext<ApplicationDbContext>(options =>
            // options.UseSqlServer(Configuration.GetConnectionString("DbConnection")));

            //services.AddDistributedMemoryCache();

            //services.AddSession(options =>
            //{
            //    // Set a short timeout for easy testing.
            //    options.IdleTimeout = TimeSpan.FromDays(1);
            //    options.Cookie.HttpOnly = true;
            //});        

            //services.AddCookieManager();


            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //   .AddEntityFrameworkStores<ApplicationDbContext>()
            //   .AddDefaultTokenProviders();

            //services.AddScoped<DbContext, ApplicationDbContext>();

            //services.AddAuthentication();

            //services.AddSingleton<IConfiguration>(Configuration);
            //services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.Configure<FormOptions>(x =>
            //{
            //    //x.ValueLengthLimit = int.MaxValue;
            //    //x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            //    //x.ValueCountLimit = 10; //default 1024
            //    //x.ValueLengthLimit = int.MaxValue; //not recommended value
            //    //x.MultipartBodyLengthLimit = long.MaxValue; //not recommended value

            //    x.ValueLengthLimit = int.MaxValue;
            //    x.MultipartBodyLengthLimit = int.MaxValue;
            //    x.MemoryBufferThreshold = int.MaxValue;

            //});

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("DbConnection")));

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
            });

            services.AddCookieManager();


            services.Configure<CookiePolicyOptions>(options =>
            {
                // this lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;

            });



            //services.AddDbContext<ApplicationDbContext>(options =>
            //   options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<DbContext, ApplicationDbContext>();

            //services.AddAuthentication()
            ////.AddCookie("Application")
            ////.AddCookie("External").
            //.AddGoogle(googleOptions =>
            //{
            //    //googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
            //    //googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            //    googleOptions.ClientId = Configuration.GetSection("Google:ClientId").Value;
            //    googleOptions.ClientSecret = Configuration.GetSection("Google:ClientSecret").Value;

            //}).AddFacebook(facebookOptions =>
            //{
            //    //facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
            //    //facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            //    facebookOptions.AppId = Configuration.GetSection("Facebook:AppId").Value;
            //    facebookOptions.AppSecret = Configuration["Facebook:AppSecret"];
            //});

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.AddSwaggerDocument();
            services.Configure<FormOptions>(x =>
            {
                //x.ValueLengthLimit = int.MaxValue;
                //x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
                //x.ValueCountLimit = 10; //default 1024
                //x.ValueLengthLimit = int.MaxValue; //not recommended value
                //x.MultipartBodyLengthLimit = long.MaxValue; //not recommended value

                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MemoryBufferThreshold = int.MaxValue;

            });
            //services.AddMvc();
            //services.AddProgressiveWebApp();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();

            app.UseAuthentication();
            // app.UseCookiePolicy();
            app.UseSession();
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
