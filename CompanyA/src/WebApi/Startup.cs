namespace WebApi
{
    using System;
    using System.IO;

    using Core.Entities;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using StartupHelpers;

    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        private const string SwaggerTitle = "Technical Test - Company A";
        private const string SwaggerVersion = "v1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(SwaggerVersion, new Info { Title = SwaggerTitle, Version = SwaggerVersion });
                c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebApi.xml"));
            });

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter(true));
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            services.Configure<ReferenceColorMatchingSettings>(Configuration.GetSection("ReferenceColorMatching"));

            return new WindsorServiceProviderBuilder().Build(services);
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
                c.SwaggerEndpoint($"/swagger/{SwaggerVersion}/swagger.json", SwaggerTitle);
                c.RoutePrefix = "swagger";
            });

            app.UseMvc();
        }
    }
}
