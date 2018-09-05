using System;
using System.IO;
using System.Reflection;
using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DataAccess.UnitOfWork;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Filters;
using KitBuilderWebApi.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace KitBuilderWebApi
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(setupAction =>
            {
                // Web API configuration and services
                setupAction.Filters.Add(new ValidateModelAttribute());
                setupAction.ReturnHttpNotAcceptable = true;
            })
                 .AddMvcOptions(o => o.OutputFormatters.Add(
                     new XmlDataContractSerializerOutputFormatter()));

            var connectionString = Configuration["connectionStrings:KitBuilderDBConnectionString"];
            services.AddDbContext<KitBuilderContext>(o => o.UseSqlServer(connectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IRepository<InstructionList>, Repository<InstructionList>>();
            services.AddScoped<IRepository<InstructionListMember>, Repository<InstructionListMember>>();
            services.AddScoped<IRepository<InstructionType>, Repository<InstructionType>>();
            services.AddScoped<IRepository<Status>, Repository<Status>>();
            services.AddScoped<IRepository<LinkGroup>, Repository<LinkGroup>>();
            services.AddScoped<IRepository<LinkGroupItem>, Repository<LinkGroupItem>>();
            services.AddScoped<IRepository<KitLinkGroup>, Repository<KitLinkGroup>>();
            services.AddScoped<IRepository<Items>, Repository<Items>>();
            services.AddScoped<IRepository<KitLinkGroupItem>, Repository<KitLinkGroupItem>>();
            services.AddScoped<IRepository<LinkGroupDto>, Repository<LinkGroupDto>>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>()
                .ActionContext;
                return new UrlHelper(actionContext);
            });
            services.AddScoped<InstructionListHelper, InstructionListHelper>();
            services.AddScoped<IHelper<ItemsDto, QueryParameters.ItemsParameters>, ItemHelper>();
            services.AddScoped<IHelper<LinkGroupDto, QueryParameters.LinkGroupParameters>, LinkGroupHelper>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "KitBuilder API", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // add services here
            //exmaple: Services.AddSingleTon <>
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // middleware to log 
            loggerFactory.AddNLog();

            // makes debugging easier--it will show error like 400 on browser
            app.UseStatusCodePages();

            AutoMapper.Mapper.Initialize(cfg =>
            { cfg.CreateMap<LinkGroup, LinkGroupDto>()
               .ForMember(dest => dest.LinkGroupItemDto, conf => conf.MapFrom(src => src.LinkGroupItem));
               
                cfg.CreateMap<LinkGroupDto, LinkGroup>();
                cfg.CreateMap<LinkGroupItem, LinkGroupItemDto>();
                cfg.CreateMap<LinkGroupItemDto, LinkGroupItem>();
                cfg.CreateMap<Items, ItemsDto>();            
                cfg.CreateMap<ItemsDto, Items>();
                cfg.CreateMap<InstructionList, InstructionListDto>();
                cfg.CreateMap<InstructionListDto, InstructionList>();
            });

            // only when environment is development show fill exception
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500,
                                exceptionHandlerFeature.Error,
                                exceptionHandlerFeature.Error.Message);
                        }

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");

                    });
                });
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "KitBuilder V1");
            });

            app.UseMvc();
        }

    }
}