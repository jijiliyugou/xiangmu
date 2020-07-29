using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using Yaeher.Authentication.JwtBearer;
using Yaeher.Configuration;
using Yaeher.Identity;
using Abp.AspNetCore.SignalR.Hubs;
using System.IO;
using Yaeher.Web.Core;
using Hangfire;
using Hangfire.MySql.Core;
using System.Data;
using Hangfire.Dashboard.BasicAuthorization;

namespace YaeherPatientAPI.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //  hangfire
            services.AddHangfire(x => x.UseStorage(new MySqlStorage(_appConfiguration["ConnectionStrings:HangfireDB"]
            , new MySqlStorageOptions
            {
                TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                QueuePollInterval = TimeSpan.FromSeconds(15),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = false,
                DashboardJobListLimit = 50000,
                TransactionTimeout = TimeSpan.FromMinutes(1),
            }
            )));

            // MVC
            services.AddMvc(

            options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory(_defaultCorsPolicyName));
                options.Filters.Add(typeof(WebApiMiddleware));
            }
            );

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            services.AddSignalR();

            //Configure CORS for angular2 UI

           services.AddCors(
               options => options.AddPolicy(
                   _defaultCorsPolicyName,
                   builder => builder
                       .WithOrigins(
                           // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                           _appConfiguration["App:CorsOrigins"]
                               .Split(",", StringSplitOptions.RemoveEmptyEntries)
                               .Select(o => o.RemovePostFix("/"))
                               .ToArray()
                       )
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials()
               )
           );

           // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
           services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "YaeherPatientAPI API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                // Assign scope requirements to operations based on AuthorizeAttribute
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                //Determine base path for the application.  
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                //Set the comments path for the swagger json and ui.  
                var xmlPath = Path.Combine(basePath, "YaeherPatientAPI.Web.Host.xml");
                options.IncludeXmlComments(xmlPath);

                var ApplicationbasexmlPath = basePath.Replace("YaeherPatientAPI.Web.Host", "Yaeher.Application");
                var ApplicationxmlPath = Path.Combine(ApplicationbasexmlPath, "Yaeher.Application.xml");
                options.IncludeXmlComments(ApplicationxmlPath);

                var CorebasexmlPath = basePath.Replace("YaeherPatientAPI.Web.Host", "Yaeher.Core");
                var CorexmlPath = Path.Combine(CorebasexmlPath, "Yaeher.Core.xml");
                options.IncludeXmlComments(CorexmlPath);

            });
            // Configure Abp and Dependency Injection
            return services.AddAbp<YaeherPatientAPIWebHostModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                {
                    RequireSsl = false,
                    SslRedirect = false,
                    LoginCaseSensitive = true,
                    Users = new []
                    {
                        new BasicAuthAuthorizationUser
                        {
                            Login = "admin",
                            PasswordClear =  "yaeheruser2018"
                        }
                    }

                }) }
            });
            app.UseAbp(options => { options.UseAbpRequestLocalization = false; }); // Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAbpRequestLocalization();

            app.UseSignalR(routes =>
            {
                routes.MapHub<AbpCommonHub>("/signalr");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            //// Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            //// Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "YaeherPatientAPI API V1");
                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("YaeherPatientAPI.Web.Host.wwwroot.swagger.ui.index.html");
            }); // URL: /swagger

            //var queues = new[] { "default", "apis", "jobs","ConsultationJob" };
            //app.UseHangfireServer(new BackgroundJobServerOptions
            //{
            //    //wait all jobs performed when BackgroundJobServer shutdown.
            //    ShutdownTimeout = TimeSpan.FromMinutes(30),
            //    Queues = queues,
            //    WorkerCount = Math.Max(Environment.ProcessorCount, 20)
            //});
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
            //    {
            //        RequireSsl = false,
            //        SslRedirect = false,
            //        LoginCaseSensitive = true,
            //        Users = new []
            //        {
            //            new BasicAuthAuthorizationUser
            //            {
            //                Login = "admin",
            //                PasswordClear =  "123456"
            //            }
            //        }

            //    }) }
            //});
        }

    }
}
