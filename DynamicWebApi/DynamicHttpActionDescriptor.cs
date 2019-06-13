using DynamicWebApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicWebApi
{
    public class DynamicHttpActionDescriptor : ReflectedHttpActionDescriptor
    {
        private readonly Collection<HttpMethod> _httpVerbs = new Collection<HttpMethod>();

        public DynamicHttpActionDescriptor(HttpControllerDescriptor controllerDescriptor, MethodInfo methodInfo)
            : base(controllerDescriptor, methodInfo)
        {
            var isDynamicController = controllerDescriptor.Properties.ContainsKey("IsDynamicController");
            if (isDynamicController)
            {
                var serviceType = controllerDescriptor.Properties["ServiceType"];
                var httpVerbAttributes = ((Type)serviceType).GetMethod(methodInfo.Name).GetCustomAttributes<Attribute>()
                    .Where(t => typeof(IActionHttpMethodProvider).IsAssignableFrom(t.GetType()))
                    .ToList();

                if (httpVerbAttributes.Any())
                {
                    //根据HttpGet/HttpPost/HttpPut/HttpDelete标签
                    if (httpVerbAttributes.Count > 1)
                        throw new Exception($"Multiple http verb matched in method {methodInfo.Name} of {((Type)serviceType).Name}");

                    var httpVerb = httpVerbAttributes.FirstOrDefault().GetType().Name;
                    switch (httpVerb)
                    {
                        case "HttpGetAttribute":
                            _httpVerbs.Add(HttpMethod.Get);
                            break;
                        case "HttpPostAttribute":
                            _httpVerbs.Add(HttpMethod.Post);
                            break;
                        case "HttpPutAttribute":
                            _httpVerbs.Add(HttpMethod.Put);
                            break;
                        case "HttpDeleteAttribute":
                            _httpVerbs.Add(HttpMethod.Delete);
                            break;
                    }
                }
                else
                {
                    //根据方法名称
                    var methodName = methodInfo.Name;
                    if (methodName.StartsWith("Get"))
                    {
                        _httpVerbs.Add(HttpMethod.Get);
                    }
                    else if (methodName.StartsWith("Create"))
                    {
                        _httpVerbs.Add(HttpMethod.Get);
                    }
                    else if (methodName.StartsWith("Update"))
                    {
                        _httpVerbs.Add(HttpMethod.Put);
                    }
                    else if (methodName.StartsWith("Delete"))
                    {
                        _httpVerbs.Add(HttpMethod.Delete);
                    }
                    else
                    {
                        var arguments = methodInfo.GetParameters();
                        if (arguments.Any(arg => !arg.GetType().IsValueType))
                        {
                            _httpVerbs.Add(HttpMethod.Post);
                        }
                        else
                        {
                            _httpVerbs.Add(HttpMethod.Post);
                        }
                    }

                }
            }

        }

        public override Collection<HttpMethod> SupportedHttpMethods
        {
            get
            {
                return _httpVerbs;
            }
        }

        public override Type ReturnType
        {
            get
            {
                return typeof(DynamicApiResult);
            }
        }

        public override Task<object> ExecuteAsync(HttpControllerContext controllerContext, IDictionary<string, object> arguments, CancellationToken cancellationToken)
        {

            return 
                base.ExecuteAsync(controllerContext, arguments, cancellationToken);
        }
    }
}