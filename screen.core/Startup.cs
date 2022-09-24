using Autofac;
using Autofac.Extensions.DependencyInjection;
using FreeSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Screen.Core
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Screen_Core", Version = "v1" });
            });
            services.AddSignalR();
            services.AddSignalR(hubOptions =>
            {
                //服务器端向客户端 ping的间隔
                hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(15);
            });

            // 注册跨域权限
            services.AddCors(c =>
               c.AddPolicy("AllowAll", p =>
               {
                   p.SetIsOriginAllowed(_ => true);     //p.AllowAnyOrigin();
                   //p.WithOrigins(cors);
                   p.AllowCredentials();
                   p.AllowAnyMethod();
                   p.AllowAnyHeader();
               })
               );

            services.AddHostedService<QuartzNetHostService>();

            var connectionString = ConfigurationHelper.GetByName("conn");

            IFreeSql fsql = new FreeSql.FreeSqlBuilder()
              .UseConnectionString(FreeSql.DataType.MySql, connectionString)
              .UseAutoSyncStructure(false)
              .Build();

            services.AddSingleton<IFreeSql>(fsql);

        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterBuildCallback((container) =>
            {
                NtiIoc.ServiceProvider = new AutofacServiceProvider(container);
            });
            builder.RegisterModule(new AutofacModuleRegister());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Screen_Core v1"));
            app.UseRouting();
            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<InterfaceServiceHub>("/InterfaceServiceHub");
            });
        }
    }


    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            var assemblysServices = Assembly.Load("Screen.Core");
            builder.RegisterAssemblyTypes(assemblysServices)
                .InstancePerDependency()
               .AsImplementedInterfaces(); //引用Autofac.Extras.DynamicProxy;

        }

    }


}
