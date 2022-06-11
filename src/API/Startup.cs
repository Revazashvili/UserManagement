using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Application;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Settings;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Netjection;

namespace API;

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
        services.InjectServices(Assembly.GetAssembly(typeof(ITokenGenerator))!,
            Assembly.GetAssembly(typeof(TokenGenerator))!, 
            Assembly.GetExecutingAssembly());

        services.AddCors(options => options.AddPolicy(ApiCorsPolicy, builder =>
            builder.AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins(Configuration.GetValue<string[]>("Cors"))
                .AllowCredentials()
        ));

        services.AddAuthorization(x =>
        {
            x.AddPolicy(JwtBearerDefaults.AuthenticationScheme, builder =>
            {
                builder.RequireAuthenticatedUser();
            });
        });

        services.AddInfrastructure(Configuration);
        services.AddApplication(Configuration);
        services.AddControllers().AddFluentValidation();

        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                var jwtSettings = services.BuildServiceProvider().GetService<JwtSettings>()!;
                x.SaveToken = true;
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.AccessTokenSecret)),
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience
                };
            });
            
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "API", Version = "v1"});
            c.EnableAnnotations();

            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
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

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
            
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}