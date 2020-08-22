using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace SignalRWebApp
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddCors(option =>
            {
                option.AddPolicy("CorsPolicy", builder =>
                         builder.WithOrigins("*", "http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddSignalR();

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(@"SignalRWebApp.xml");
                c.SwaggerDoc("v1", new OpenApiInfo { Title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, Version = "v1" });
            });

            #endregion Swagger
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseSignalR(routes =>
            {
                routes.MapHub<SignalHub>("/pushNotification");
            });

            app.UseHttpsRedirection();
            app.UseMvc();

            #region Swagger

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SignalR Demo V1");
            });

            #endregion Swagger
        }
    }
}
