using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle;
using Castle.DynamicProxy;
using Castle.Core.Interceptor;
using System.ServiceModel.Channels;
using System.ServiceModel;
using Newtonsoft.Json;

namespace Client
{
    public class CastleServicePorxy<TService> : IInterceptor
    {
        private readonly Binding _binding;
        private readonly EndpointAddress _endpointAddress;

        public CastleServicePorxy(Binding binding, EndpointAddress endpointAddress)
        {
            _binding = binding;
            _endpointAddress = endpointAddress;
        }

        public CastleServicePorxy(Binding binding, string serviceUrl)
            : this(binding, new EndpointAddress(serviceUrl))
        {

        }

        public void Intercept(IInvocation invocation)
        {
            var serviceInfo = FindService();
            var methodInfo = invocation.Method;
            var startTime = DateTime.Now;
            var serviceName = serviceInfo.Service.GetType().Name;
            var methodName = methodInfo.Name;

            try
            {
                Console.WriteLine("CastleProxy调用{0}服务{1}方法开始...", serviceName, methodName);
                //var argsInfo = new Dictionary<string, object>();
                //for (int i = 0; i < invocation.Arguments.Length; i++)
                //{
                //    argsInfo.Add(invocation.,invocation.Arguments[i]);
                //}
                //Console.WriteLine("当前传入参数:{0}", JsonConvert.SerializeObject(argsInfo));
                var result = methodInfo.Invoke(serviceInfo.Service, invocation.Arguments);
                if (result != null) { 
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

        private ServiceInfo<TService> FindService()
        {
            ChannelFactory<TService> channelFactory = new ChannelFactory<TService>(_binding, _endpointAddress);
            var serviceInfo = new ServiceInfo<TService>(channelFactory);
            serviceInfo.Service = channelFactory.CreateChannel();
            return serviceInfo;
        }
    }
}
