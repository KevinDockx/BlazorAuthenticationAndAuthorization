using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BethanysPieShopHRM.Server.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;

namespace BethanysPieShopHRM.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor().AddCircuitOptions(options => 
            { options.DetailedErrors = true; });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
                options =>
                {
                    options.Authority = "https://localhost:44333";
                    options.ClientId = "bethanyspieshophr";
                    options.ClientSecret = "108B7B4F-BEFC-4DD2-82E1-7F025F0F75D0";
                    options.ResponseType = "code id_token";
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.Scope.Add("bethanyspieshophrapi");
                    options.Scope.Add("country");
                    options.ClaimActions.MapUniqueJsonKey("country", "country");
                    //options.CallbackPath = ...
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                });

            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy(
                    BethanysPieShopHRM.Shared.Policies.CanManageEmployees,
                    BethanysPieShopHRM.Shared.Policies.CanManageEmployeesPolicy());
            });


            services.AddHttpClient<IEmployeeDataService, EmployeeDataService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44340/");
            });
            services.AddHttpClient<ICountryDataService, CountryDataService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44340/");
            });
            services.AddHttpClient<IJobCategoryDataService, JobCategoryDataService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44340/");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
