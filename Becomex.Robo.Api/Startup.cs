using AutoMapper;
using Becomex.Robo.Application;
using Becomex.Robo.Application.Filters;
using Becomex.Robo.Infra.IoC;
using Bexomex.Robo.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Newtonsoft;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace Becomex.Robo.Api
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
            services.AddControllers();

            services.AddCors(options => {
                options.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });

            // Enable Health Checks
            services.AddHealthChecks();

            // Configuring HTTP Response Compression
            services.AddResponseCompression(
                options =>
                {
                    options.Providers.Add<BrotliCompressionProvider>();
                    options.Providers.Add<GzipCompressionProvider>();
                });
            services.Configure<BrotliCompressionProviderOptions>(
                options =>
                {
                    options.Level = CompressionLevel.Fastest;
                });
            services.Configure<GzipCompressionProviderOptions>(
                options =>
                {
                    options.Level = CompressionLevel.Fastest;
                });

            // Set versioning
            services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;

                    // automatically applies an api version based on the name of the defining controller's namespace
                    options.Conventions.Add(new VersionByNamespaceConvention());

                    options.AssumeDefaultVersionWhenUnspecified = true;
                });
            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;

                    options.AssumeDefaultVersionWhenUnspecified = true;
                });

            services
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.RespectBrowserAcceptHeader = true;
                    options.Filters.Add(new ApplicationExceptionFilter());
                });


            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(
                options =>
                {
                    // resolve the IApiVersionDescriptionProvider service
                    // note: that we have to build a temporary service provider here because one has not been created yet
                    using (var serviceProvider = services.BuildServiceProvider())
                    {
                        var provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

                        // add a swagger document for each discovered API version
                        // note: you might choose to skip or document deprecated API versions differently
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                        }
                    }

                    // integrate xml comments
                    options.IncludeXmlComments(XmlCommentsFilePath);
                });

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddAutoMapper(typeof(DomainMappingProfile).Assembly);
            services.AddDIConfiguration();
            services.AddHttpClient();
            services.AddMemoryCache();
            services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider, IMemoryCache cache)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                    options.RoutePrefix = string.Empty;
                });

            app.UseMvc();

            // Create new robot list in cache
            var entryOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove);
            cache.Set("Robots", new List<Robot>() { new Robot() { Description = "Robô para ser utilizado como exemplo.", Name = "Robô Exemplo" } }, entryOptions);
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                return Path.Combine(AppContext.BaseDirectory, xmlFile);
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "R.O.B.O API",
                Version = description.ApiVersion.ToString(),
                Description = "A sample application for Binary Oriented Operating Robot (R.O.B.O).",
                Contact = new OpenApiContact { Name = "Luiz Antonio Lima", Email = "luizantonio.fyc@gmail.com" }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
