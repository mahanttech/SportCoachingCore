using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Api.Extensions;
using Web.Api.Infrastructure.Repository;
using Web.Api.Infrastructure.Interface;
using Web.Api.Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Web.Api.Core.Entity;
using AutoMapper;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Web.Api.Core.Context;

namespace Web.Api
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
            var connection = @"" + Constants.connection;
            services.AddCors();
            services.AddDbContext<SportTestContext>(options => options.UseSqlServer(connection));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper();
            AutoMapperConfiguration.Configure();
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<IErrorLog, ErrorLogsHelper>();


            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.Events = new JwtBearerEvents
               {
                   OnTokenValidated = context =>
                   {
                       var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                       var userId = int.Parse(context.Principal.Identity.Name);
                       var user = userService.GetById(userId);
                       if (user == null)
                       {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                       }
                       return Task.CompletedTask;
                   }
               };
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();



            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Web.Api", Version = "v1" });
            });
           
          
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
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(
                              async context =>
                              {
                                  context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                  context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                                  var error = context.Features.Get<IExceptionHandlerFeature>();
                                  if (error != null)
                                  {
                                      context.Response.AddApplicationError(error.Error.Message);
                                      await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                                  }
                              });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseCookiePolicy();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.

            app.UseSwaggerUI(c =>
            {
               c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestAPI V1");
                //c.SwaggerEndpoint("/swagger/v1", "TestAPI V1");
            });

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseMvc();

        }
    }
}
