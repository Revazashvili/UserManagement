using System.Linq;
using System.Net;
using System.Text.Json;
using Application;
using Application.Common.Models;
using FluentValidation.AspNetCore;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        
        private const string ApiCorsPolicy = "APICorsPolicy";
        public void ConfigureServices(IServiceCollection services)
        {
            var cors = Configuration.GetValue<string[]>("ApiCorsPolicy");
            services.AddCors(options => options.AddPolicy(ApiCorsPolicy, builder =>
            {
                builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(cors)
                    .AllowCredentials();
            }));

            services.AddHttpContextAccessor();
            
            services.AddInfrastructure(Configuration);
            services.AddApplication(Configuration);
            
            services.AddControllers().AddFluentValidation(options => options.AutomaticValidationEnabled = true);;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "API", Version = "v1"});
                c.EnableAnnotations();
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (contextFeature != null)
                    {
                        var result = JsonSerializer.Serialize(Response.Fail<object>(
                            $"{contextFeature.Error?.Message} {contextFeature.Error?.InnerException?.Message}"));
                        logger.LogError("Error occured {error} {@result}",contextFeature.Error, result);
                        await context.Response.WriteAsync(result);
                    }
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}