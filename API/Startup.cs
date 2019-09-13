using System;
using API.CrossCutting.DependencyInjection;
using API.Domain.Repository;
using API.Domain.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace API
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
            ConfigureService.ConfigureDependenciesService(services);
            ConfigureRepository.ConfigureDependenciesRepository(services);

            var signingConfiguration = new SigningConfiguration();
            services.AddSingleton(signingConfiguration);

            var tokenConfiguration = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(
                Configuration.GetSection("TokenConfigurations"))
                .Configure(tokenConfiguration);
            services.AddSingleton(tokenConfiguration);

            services.AddAuthentication(authOptions =>
           {
               authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           }).AddJwtBearer(bearerOptions =>
           {
               var paramsValidation = bearerOptions.TokenValidationParameters;
               paramsValidation.IssuerSigningKey = signingConfiguration.Key;
               paramsValidation.ValidAudience = tokenConfiguration.Audience;
               paramsValidation.ValidIssuer = tokenConfiguration.Issuer;
               paramsValidation.ValidateIssuerSigningKey = true;
               paramsValidation.ValidateLifetime = true;
               paramsValidation.ClockSkew = TimeSpan.Zero;
           });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "API AspNetCore 2.2",
                        Version = "v1",
                        Description = "Nova API utilizando AspNetCore 2.2",
                        Contact = new Contact
                        {
                            Name = "Tiago Gilli",
                            Url = "https://github.com/tiagogilli99/API-ApsNetCore-2.2"
                        }

                    });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
           {
               c.RoutePrefix = string.Empty;
               c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nova API utilizando AspNetCore 2.2");
           });

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);

            app.UseMvc();
        }
    }
}
