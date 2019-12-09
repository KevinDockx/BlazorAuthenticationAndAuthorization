// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using Marvin.IDP.Areas.Identity.Data;
using Marvin.IDP.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;
using System.Security.Claims;

namespace Marvin.IDP
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                // uncomment to write to Azure diagnostics stream
                //.WriteTo.File(
                //    @"D:\home\LogFiles\Application\identityserver.txt",
                //    fileSizeLimitBytes: 1_000_000,
                //    rollOnFileSizeLimit: true,
                //    shared: true,
                //    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

            try
            {
                Log.Information("Starting host...");
                var host = CreateHostBuilder(args).Build();

                // seed the database.  Best practice = in Main, using service scope
                using (var scope = host.Services.CreateScope())
                {
                    try
                    {
                        var context = scope.ServiceProvider.GetService<UserDbContext>();

                        // ensure the DB is migrated before seeding
                        context.Database.Migrate();

                        // use the user manager to create test users
                        var userManager = scope.ServiceProvider
                            .GetRequiredService<UserManager<ApplicationUser>>();

                        var jack = userManager.FindByNameAsync("Jack").Result;
                        if (jack == null)
                        {
                            jack = new ApplicationUser
                            {
                                UserName = "Jack",
                                EmailConfirmed = true
                            };

                            var result = userManager.CreateAsync(jack, "P@ssword1").Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }

                            result = userManager.AddClaimsAsync(jack, new Claim[]{
                                new Claim(JwtClaimTypes.Name, "Jack Torrance"),
                                new Claim(JwtClaimTypes.GivenName, "Jack"),
                                new Claim(JwtClaimTypes.FamilyName, "Torrance"),
                                new Claim(JwtClaimTypes.Email, "jack.torrance@email.com"),
                                new Claim("country", "BE")
                            }).Result;

                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                        }

                        var wendy = userManager.FindByNameAsync("Wendy").Result;
                        if (wendy == null)
                        {
                            wendy = new ApplicationUser
                            {
                                UserName = "Wendy",
                                EmailConfirmed = true
                            };

                            var result = userManager.CreateAsync(wendy, "P@ssword1").Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }

                            result = userManager.AddClaimsAsync(wendy, new Claim[]{
                                new Claim(JwtClaimTypes.Name, "Wendy Torrance"),
                                new Claim(JwtClaimTypes.GivenName, "Wendy"),
                                new Claim(JwtClaimTypes.FamilyName, "Torrance"),
                                new Claim(JwtClaimTypes.Email, "wendy.torrance@email.com"),
                                new Claim("country", "NL")
                            }).Result;

                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while seeding the database.");
                    }
                }

                // run the web app
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog();
                });
    }
}