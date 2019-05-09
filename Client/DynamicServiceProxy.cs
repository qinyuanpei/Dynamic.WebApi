using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using System.Reflection;
using System.ServiceModel.Channels;
using Newtonsoft.Json;

namespace Client
{
    public class DynamicServiceProxy<TService> : RealProxy
    {
        private readonly Binding _binding;
        private readonly EndpointAddress _endpointAddress;

        public DynamicServiceProxy(Binding binding, EndpointAddress endpointAddress)
            : base(typeof(TService))
        {
            _binding = binding;
            _endpointAddress = endpointAddress;
        }

        public DynamicServiceProxy(Binding binding, string serviceUrl)
            : this(binding, new EndpointAddress(serviceUrl))
        {

        }

        public override IMessage Invoke(IMessage message)
        {
            var serviceInfo = FindService();
            var methodCall = message as IMethodCallMessage;
            var methodInfo = methodCall.MethodBase as MethodInfo;
            var startTime = DateTime.Now;
            var serviceName = serviceInfo.Service.GetType().Name;
            var methodName = methodInfo.Name;

            try
            {
                Console.WriteLine("调用{0}服务{1}方法开始...", serviceName, methodName);
                var argsInfo = new Dictionary<string, object>();
                for (int i = 0; i < methodCall.ArgCount; i++)
                {
                    argsInfo.Add(methodCall.GetArgName(i), methodCall.Args[i]);
                }
                Console.WriteLine("当前传入参数:{0}", JsonConvert.SerializeObject(argsInfo));
                var result = methodInfo.Invoke(serviceInfo.Service, methodCall.InArgs);
                if (result != null)
                    Console.WriteLine("当前返回值:{0}", JsonConvert.SerializeObject(result));
                return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
            }
            catch (Exception ex)
            {
                Console.WriteLine("调用{0}服务{1}方法失败,失败原因：{2}", serviceName, methodName, ex.Message);
                throw ex;
            }
            finally
            {
                serviceInfo.Close();
                Console.WriteLine("调用{0}服务{1}方法结束,共耗时{2}秒", serviceName, methodName, DateTime.Now.Subtract(startTime).TotalSeconds);
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
