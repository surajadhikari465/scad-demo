using IrmaMobile.Services;
using LoggerMiddleware.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IrmaMobile
{
    public class Startup
    {
        private bool enableAuthentication = false;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            enableAuthentication = bool.Parse(Configuration["EnableAuthentication"]);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            if (enableAuthentication)
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
            }

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            services.AddControllers();

            services.Configure<AppConfiguration>(Configuration.GetSection("IrmaMobile"));
            services.AddSingleton<IIrmaMobileService, IrmaMobileService>();
            
            services.AddLoggerAccessor();

            services.AddMvc(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.AddRequestResponseLoggingMiddleware(Configuration);

            app.UseHealthChecks("/healthcheck");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            if (enableAuthentication)
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }
            app.UseCors("AllowAll");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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