﻿using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Autofac;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.AffiliateApi.Grpc;
using MarketingBox.AffiliateApi.Modules;
using MarketingBox.AffiliateApi.Services;
using MarketingBox.AffiliateApi.Swagger;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyJetWallet.Sdk.GrpcSchema;
using MyJetWallet.Sdk.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Prometheus;
using SimpleTrading.ServiceStatusReporterConnector;
using SimpleTrading.Telemetry;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MarketingBox.AffiliateApi
{
    public class Startup
    {
        public Startup()
        {
            ModelStateDictionaryResponseCodes = new HashSet<int>();

            ModelStateDictionaryResponseCodes.Add(StatusCodes.Status400BadRequest);
            ModelStateDictionaryResponseCodes.Add(StatusCodes.Status500InternalServerError);
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.BindCodeFirstGrpc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicies.AffiliateAndHigher, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role);
                    policy.Requirements.Add(new RolesAuthorizationRequirement(new []
                    {
                        UserRole.Affiliate.ToString(),
                        UserRole.AffiliateManager.ToString(),
                        UserRole.MasterAffiliate.ToString(),
                        UserRole.MasterAffiliateReferral.ToString(),
                        UserRole.Admin.ToString()
                    }));
                });

                options.AddPolicy(AuthorizationPolicies.MasterAffiliateAndHigher, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role);
                    policy.Requirements.Add(new RolesAuthorizationRequirement(new[]
                    {
                        UserRole.AffiliateManager.ToString(),
                        UserRole.MasterAffiliate.ToString(),
                        UserRole.MasterAffiliateReferral.ToString(),
                        UserRole.Admin.ToString()
                    }));
                });

                options.AddPolicy(AuthorizationPolicies.AffiliateManagerAndHigher, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role);
                    policy.Requirements.Add(new RolesAuthorizationRequirement(new[]
                    {
                        UserRole.AffiliateManager.ToString(),
                        UserRole.Admin.ToString()
                    }));
                });

                options.AddPolicy(AuthorizationPolicies.AdminOnly, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role);
                    policy.Requirements.Add(new RolesAuthorizationRequirement(new[]
                    {
                        UserRole.Admin.ToString()
                    }));
                });
            });
            services.AddControllers().AddNewtonsoftJson(ConfigureMvcNewtonsoftJsonOptions);
            services.AddSwaggerGen(ConfigureSwaggerGenOptions);
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddHostedService<ApplicationLifetimeManager>();

            services
                .AddAuthentication(ConfigureAuthenticationOptions)
                .AddJwtBearer(ConfigureJwtBearerOptions);

            services.BindTelemetry("AffiliateApi", "MB-", Program.Settings.JaegerUrl);
        }

        protected virtual void ConfigureJwtBearerOptions(JwtBearerOptions options)
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Program.Settings.JwtSecret)),
                ValidateIssuer = false,
                ValidateAudience = true,
                ValidAudience = Program.Settings.JwtAudience,
                ValidateLifetime = true
            };
        }

        protected virtual void ConfigureAuthenticationOptions(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMetricServer();

            app.BindServicesTree(Assembly.GetExecutingAssembly());

            app.BindIsAlive();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcSchema<HelloService, IHelloService>();

                endpoints.MapGrpcSchemaRegistry();

                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });

            app.UseSwagger(c => c.RouteTemplate = "api/{documentName}/swagger.json");
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../../api/v1/swagger.json", "API V1");
                c.RoutePrefix = "swagger/ui";
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<SettingsModule>();
            builder.RegisterModule<ServiceModule>();
        }
        public ISet<int> ModelStateDictionaryResponseCodes { get; }
        protected virtual void ConfigureSwaggerGenOptions(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "MarketingBox.AffiliateApi", Version = "v1" });
            options.EnableXmsEnumExtension();
            options.MakeResponseValueTypesRequired();

            foreach (var code in ModelStateDictionaryResponseCodes)
            {
                options.AddModelStateDictionaryResponse(code);
            }

            options.AddJwtBearerAuthorization();
        }

        protected virtual void ConfigureMvcNewtonsoftJsonOptions(MvcNewtonsoftJsonOptions options)
        {
            var namingStrategy = new CamelCaseNamingStrategy();

            options.SerializerSettings.Converters.Add(new StringEnumConverter(namingStrategy));
            options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            options.SerializerSettings.Culture = CultureInfo.InvariantCulture;
            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Error;
            options.SerializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = namingStrategy
            };
        }
    }
}
