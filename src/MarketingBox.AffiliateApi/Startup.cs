using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using AutoWrapper;
using MarketingBox.AffiliateApi.Modules;
using MarketingBox.Sdk.Common.Models.RestApi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MyJetWallet.Sdk.GrpcSchema;
using MyJetWallet.Sdk.Service;
using Prometheus;
using SimpleTrading.ServiceStatusReporterConnector;
using SimpleTrading.Telemetry;

namespace MarketingBox.AffiliateApi
{
    public class Startup
    {
        private readonly string _corsPolicy = "Develop";

        public Startup()
        {
            ModelStateDictionaryResponseCodes = new HashSet<int>();

            ModelStateDictionaryResponseCodes.Add(StatusCodes.Status400BadRequest);
            ModelStateDictionaryResponseCodes.Add(StatusCodes.Status500InternalServerError);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.BindCodeFirstGrpc();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy(_corsPolicy,
            //        builder =>
            //        {
            //            builder
            //                .WithOrigins("http://localhost:3001", "http://localhost:3002", "http://localhost:3000")
            //                .AllowCredentials()
            //                .AllowAnyHeader()
            //                .AllowAnyMethod();
            //        });
            //});

            services.AddAuthorization();
            services.AddControllers();
            services.SetupSwaggerDocumentation();

            services.AddHostedService<ApplicationLifetimeManager>();

            services
                .AddAuthentication(ConfigureAuthenticationOptions)
                .AddJwtBearer(ConfigureJwtBearerOptions);

            services.BindTelemetry("AffiliateApi", "MB-", Program.Settings.JaegerUrl);

            services.AddAutoMapper(typeof(Startup));
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

            app.UseCors(_corsPolicy);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseApiResponseAndExceptionWrapper<ApiResponseMap>(
               new AutoWrapperOptions
               {
                   UseCustomSchema = true,
                   IgnoreWrapForOkRequests = true
               });

            app.UseMetricServer();

            app.BindServicesTree(Assembly.GetExecutingAssembly());

            app.BindIsAlive();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcSchemaRegistry();

                endpoints.MapControllers();

                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync(
                            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                    });
            });

            app.UseOpenApi(settings => { settings.Path = $"/swagger/api/swagger.json"; });

            app.UseSwaggerUi3(settings =>
            {
                settings.EnableTryItOut = true;
                settings.Path = $"/swagger/api";
                settings.DocumentPath = $"/swagger/api/swagger.json";
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<SettingsModule>();
            builder.RegisterModule<ClientModule>();
            builder.RegisterModule<ServiceModule>();
        }

        public ISet<int> ModelStateDictionaryResponseCodes { get; }
    }
}