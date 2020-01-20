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
using System.Threading;
using Winton.Extensions.Configuration.Consul;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Extensions.Logging;
using Serilog.Events;
using WebApiContrib.Core.Formatter;
using WebApiContrib.Core.Formatter.MessagePack;
using Microsoft.AspNetCore.HttpOverrides;
using CSRedis;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DynamicWebApi.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .MinimumLevel.Debug()
               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
               {
                   MinimumLogEventLevel = LogEventLevel.Verbose,
                   AutoRegisterTemplate = true
               })
            .CreateLogger();
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

            services.AddRazorPages();
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            services.AddControllers().AddMessagePackFormatters().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddControllersWithViews();
            services.AddMvc(opt => opt.EnableEndpointRouting = false).AddNewtonsoftJson();
            services.AddMvcCore().AddApiExplorer();
            services.AddOptions();
            services.AddMvc(opt => opt.EnableEndpointRouting = false);
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "ynamic WebApi", Version = "v1.0" });

                swagger.DocInclusionPredicate((docName, description) => true);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);
            });

            services.AddDyanmicController();

            //注册Consul
            services.AddConsul(Configuration);

            //注册Grpc服务端
            var gRpcServerPort = Configuration.GetValue<int>("AppSettings:Port");
            services.AddGrpcServer(new GrpcServerOptions() { Host = "0.0.0.0", Port = gRpcServerPort })
                .AddGrpcService<GreetRpcService>()
                .AddGrpcService<UserRpcService>();

            //注册Grpc客户端
            services.AddGrpcClient<IGreetRpcServiceClient>(
                new Grpc.Core.Channel("172.16.100.24:2345", Grpc.Core.ChannelCredentials.Insecure));
            services.AddGrpcClient<IUserRpcServiceClient>(
                new Grpc.Core.Channel("172.16.100.24:2345", Grpc.Core.ChannelCredentials.Insecure));

            //注册服务发现组件
            services.AddSingleton<IServiceDiscover, ServiceDiscover>();

            //注册CSRedis
            services.AddSingleton<CSRedisClient>(x =>
            {
                return new CSRedisClient("127.0.0.1,defaultDatabase=0,poolsize=3,tryit=0");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
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

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            app.UseGrpcServer();
        }
    }
}


