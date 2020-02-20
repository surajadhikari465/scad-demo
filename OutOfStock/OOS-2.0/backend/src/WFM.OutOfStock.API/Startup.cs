using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WFM.OutOfStock.API.Services;

namespace WFM.OutOfStock.API
{
    public class Startup
    {
        private const string ApiVersion = "v1";
        private const string ApiName = "Out Of Stock Mobile Backend API";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var jwtConfiguration = CreateConfigurationServiceInstance(Configuration);
            var authenticationKey = GetJsonWebKey(jwtConfiguration.AuthenticationServiceUrl, jwtConfiguration.JwtKeyId).Result;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = false,
                    RequireSignedTokens = true,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    IssuerSigningKey = authenticationKey,
                    ValidIssuer = jwtConfiguration.JwtTokenIssuer,
                };
                options.Audience = jwtConfiguration.JwtTokenAudience;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApiVersion, new Info
                {
                    Title = ApiName,
                    Version = ApiVersion,
                    Description = "Backend API to support the Out Of Stock Mobile Web UI. Acts as a proxy to the legacy Out of Stock WCF service."
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<AppConfiguration>(Configuration.GetSection("OutOfStock"));
            services.AddSingleton<IOutOfStockService, OutOfStockService>();
        }

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

            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{ApiName} {ApiVersion}");
            });
        }

        private static async Task<JsonWebKey> GetJsonWebKey(Uri authenticationServiceRootUrl, string dvoServicesJwtKeyId)
        {
            // Get the Json Web Key, retry 10 times if needed.
            var retryCounter = 10;
            while (retryCounter > 0)
            {
                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        Uri url = new Uri(authenticationServiceRootUrl, "api/Authentication/Jwks");
                        using (var httpResponse = await httpClient.GetAsync(url))
                        {
                            httpResponse.EnsureSuccessStatusCode();
                            var keys = await httpResponse.Content.ReadAsAsync<JsonWebKeySet>();
                            return keys.Keys.First(k => k.Kid == dvoServicesJwtKeyId);
                        }
                    }
                }
                catch
                {
                    if (retryCounter == 0)
                    {
                        throw;
                    }
                }

                Thread.Sleep(5000);
                retryCounter--;
            }

            throw new TimeoutException("Unable to get JsonWebKey from Auth Service");
        }

        private static JwtConfiguration CreateConfigurationServiceInstance(IConfiguration configuration)
        {
            return new JwtConfiguration
            {
                AuthenticationServiceUrl = new Uri(configuration["AuthenticationServiceUrl"]),
                JwtKeyId = configuration["JwtKeyId"],
                JwtTokenIssuer = configuration["JwtTokenIssuer"],
                JwtTokenAudience = configuration["JwtTokenAudience"],
            };
        }

        public class JwtConfiguration
        {
            /// <summary>
            /// Gets or sets the Authentication Service Url
            /// </summary>
            public Uri AuthenticationServiceUrl { get; set; }

            /// <summary>
            /// Gets or sets the JwtKeyId for Dvo Services
            /// </summary>
            public string JwtKeyId { get; set; }

            /// <summary>
            /// Gets or sets the Jwt Token Issuer
            /// </summary>
            public string JwtTokenIssuer { get; set; }

            /// <summary>
            /// Gets or sets the Jwt token audience
            /// </summary>
            public string JwtTokenAudience { get; set; }
        }
    }
}
