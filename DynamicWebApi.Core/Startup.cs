using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DynamicWebApi.Core.Extends;
using System.IO;
using System.Reflection;
using DynamicWebApi.Core.Services;
using DynamicWebApi.Core.Services.Rpc.Greet;
using static DynamicWebApi.Core.Services.Rpc.Greet.IGreetRpcService;
using static DynamicWebApi.Core.Services.Rpc.User.IUserRpcService;

namespace DynamicWebApi.Core
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvcCore().AddApiExplorer();
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info()
                {
                    Title = "Dynamic WebApi",
                    Version = "1.0",
                });

                swagger.DocInclusionPredicate((docName, description) => true);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);
            });

            services.AddDyanmicController();

            //注册Consul
            services.AddConsul(Configuration);

            //注册Grpc服务端
            services.AddGrpcServer()
                .AddGrpcService<GreetRpcService>()
                .AddGrpcService<UserRpcService>();

            //注册Grpc客户端
            services.AddGrpcClient<IGreetRpcServiceClient>(
                new Grpc.Core.Channel("172.16.100.24:2345", Grpc.Core.ChannelCredentials.Insecure));
            services.AddGrpcClient<IUserRpcServiceClient>(
                new Grpc.Core.Channel("172.16.100.24:2345", Grpc.Core.ChannelCredentials.Insecure));

            //注册服务发现组件
            services.AddSingleton<IServiceDiscover, ServiceDiscover>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseGrpcServer();
        }
    }
}


