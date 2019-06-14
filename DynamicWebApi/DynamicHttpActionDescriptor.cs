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
                    //根据路由来获取Http动词
                    if (httpVerbAttributes.Count > 1)
                        throw new Exception($"Multiple http verb matched in method {methodInfo.Name} of {((Type)serviceType).Name}");

                    _httpVerbs = GetHttpVerbByRoute(httpVerbAttributes);
                }
                else
                {
                    //根据方法名称获取Http动词
                    _httpVerbs = GetHttpVerbByMethod(methodInfo);

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
            return base.ExecuteAsync(controllerContext, arguments, cancellationToken)
                       .ContinueWith(task =>
                       {
                           try
                           {
                               if (task.Result == null)
                               {
                                   return new DynamicApiResult() { Flag = true };
                               }

                               if (task.Result is DynamicApiResult)
                               {
                                   return task.Result;
                               }

                               return new DynamicApiResult() { Flag = true, Result = task.Result };
                           }
                           catch (AggregateException ex)
                           {
                               throw ex;
                           }
                       });
        }

        private Collection<HttpMethod> GetHttpVerbByRoute(List<Attribute> httpVerbAttributes)
        {
            var httpVerbs = new Collection<HttpMethod>();

            var httpVerb = httpVerbAttributes.FirstOrDefault().GetType().Name;
            switch (httpVerb)
            {
                case "HttpGetAttribute":
                    httpVerbs.Add(HttpMethod.Get);
                    break;
                case "HttpPostAttribute":
                    httpVerbs.Add(HttpMethod.Post);
                    break;
                case "HttpPutAttribute":
                    httpVerbs.Add(HttpMethod.Put);
                    break;
                case "HttpDeleteAttribute":
                    httpVerbs.Add(HttpMethod.Delete);
                    break;
            }

            return httpVerbs;
        }

        private Collection<HttpMethod> GetHttpVerbByMethod(MethodInfo methodInfo)
        {
            var httpVerbs = new Collection<HttpMethod>();

            //根据方法名称
            var methodName = methodInfo.Name;
            if (methodName.StartsWith("Get"))
            {
                httpVerbs.Add(HttpMethod.Get);
            }
            else if (methodName.StartsWith("Create"))
            {
                httpVerbs.Add(HttpMethod.Get);
            }
            else if (methodName.StartsWith("Update"))
            {
                httpVerbs.Add(HttpMethod.Put);
            }
            else if (methodName.StartsWith("Delete"))
            {
                httpVerbs.Add(HttpMethod.Delete);
            }
            else
            {
                var arguments = methodInfo.GetParameters();
                if (arguments.Any(arg => !arg.GetType().IsValueType))
                {
                    httpVerbs.Add(HttpMethod.Post);
                }
                else
                {
                    httpVerbs.Add(HttpMethod.Post);
                }
            }

            return httpVerbs;
        }
    }
}