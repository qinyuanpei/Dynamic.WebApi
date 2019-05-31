using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;

namespace DynamicWebApi
{
    public class DynamciApiInterceptor<TService>: IInterceptor
    {
        private readonly TService _realObject;
        public DynamciApiInterceptor(TService realObject)
        {
            _realObject = realObject;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                var methodInfo = invocation.Method;
                var arguments = invocation.Arguments;
                var result = methodInfo.Invoke(_realObject, arguments);
                if (result != null)
                {
                    invocation.ReturnValue = typeof(JsonResult<>).MakeGenericType(typeof()
                }

                Console.WriteLine("CastleProxy调用{0}服务{1}方法开始...", serviceName, methodName);
                var argsInfo = new Dictionary<string, object>();
                var parameters = methodInfo.GetParameters();
                for (int i = 0; i < invocation.Arguments.Length; i++)
                {
                    argsInfo.Add(parameters[i].Name, invocation.Arguments[i]);
                }
                Console.WriteLine("当前传入参数:{0}", JsonConvert.SerializeObject(argsInfo));
                var result = methodInfo.Invoke(serviceInfo.Service, invocation.Arguments);
                if (result != null)
                {
                    Console.WriteLine("当前返回值:{0}", JsonConvert.SerializeObject(result));
                    invocation.ReturnValue = result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CastleProxy调用{0}服务{1}方法失败,失败原因：{2}", serviceName, methodName, ex.Message);
                throw ex;
            }
            finally
            {
                serviceInfo.Close();
                Console.WriteLine("CastleProxy调用{0}服务{1}方法结束,共耗时{2}秒", serviceName, methodName, DateTime.Now.Subtract(startTime).TotalSeconds);
                Console.WriteLine("----------------------------------");
            }
        }
    }
}