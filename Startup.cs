﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using WebApiUploadDownload.Data;
using WebApiUploadDownload.Models;
using WebApiUploadDownload.Services;

namespace WebApiUploadDownload
{
    public class Startup
    {
        public static readonly LoggerFactory MyDebugLoggerFactory
            = new LoggerFactory(new[] {
                new DebugLoggerProvider()
            });

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddDbContext<WebApiUploadDownloadContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("WebApiUploadDownloadContext"));
                options.UseLoggerFactory(MyDebugLoggerFactory);
                options.EnableSensitiveDataLogging(true);
            }
                );

            services.Configure<UploadConfig>(Configuration.GetSection("UploadConfig"));

            services.AddTransient<IFileServerProvider, LocalFileServerProvider>();
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddDbContext<WebApiUploadDownloadContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("WebApiUploadDownloadContext"));
            }
                );

            services.Configure<UploadConfig>(Configuration.GetSection("UploadConfig"));

            services.AddTransient<IFileServerProvider, AzureFileServerProvider>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
