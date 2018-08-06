using Announcements.Entities;
using Announcements.Entities.Contexts;
using Announcements.Helpers.Extensions;
using Announcements.Models;
using Announcements.Services;
using AnnouncementsAPI.Services;
using AspNetCoreRateLimit;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NSwag;
using NSwag.AspNetCore;
using System.Collections.Generic;
using System.Reflection;

namespace Announcements
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            var connectionString = Configuration["ConnectionString:AnouncementsConnectionString"];
            services.AddDbContext<AnnouncementsDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IAnnouncementsRepository, AnnouncementsRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddTransient<ITypeHelperService, TypeHelperService>();

            //services.AddResponseCaching();
            //services.AddHttpCacheHeaders((expirationModelOptions) =>
            //{
            //    expirationModelOptions.MaxAge = 300;
            //},
            //(validationModelOptions) =>
            //{
            //    validationModelOptions.MustRevalidate = true;
            //});

            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>((options) =>
            {
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Limit = 1000,
                        Period = "5m"
                    }
                };
            });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, AnnouncementsDbContext announcementsDbContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logger"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandler != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500, exceptionHandler.Error, exceptionHandler.Error.Message);
                        }

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unxpected error happened. Try again later.");
                    });
                });
                app.UseHsts();
            }

            // Mapping entities and view models
            AutoMapping();

            // Seeding fake data
            announcementsDbContext.SeedAnnouncementsData();

            app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling =
                    PropertyNameHandling.CamelCase;
            });

            app.UseSwagger(typeof(Startup).Assembly, settings =>
            {
                settings.PostProcess = document =>
                {
                    document.Info.Version = "V1";
                    document.Info.Title = "Announcements API";
                    document.Info.Description = "Web API for announcements";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new SwaggerContact
                    {
                        Name = "David Chkhitunidze",
                        Email = "davidchkhitunidze@gmail.com"
                    };
                    document.Info.License = new SwaggerLicense
                    {
                        Name = "License",
                        Url = "https://example.com/license"
                    };
                };
            }); 

            app.UseIpRateLimiting();
            //app.UseResponseCaching();
            //app.UseHttpCacheHeaders();
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void AutoMapping()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Announcement, AnnouncementForGetting>();
                cfg.CreateMap<AnnouncementForCreation, Announcement>();
                cfg.CreateMap<AnnouncementForUpdate, Announcement>();
                cfg.CreateMap<Announcement, AnnouncementForUpdate>();
            });
        }
    }
}
