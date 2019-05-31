using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class CalculatorServiceProxy : RealProxy
    {
        private Server.Service.ICalculator _calculator;

        public CalculatorServiceProxy(Server.Service.ICalculator calculator) :
            base(typeof(Server.Service.ICalculator))
        {
            _calculator = calculator;
        }

        public override IMessage Invoke(IMessage message)
        {
            var methodCall = message as IMethodCallMessage;
            var methodInfo = methodCall.MethodBase as MethodInfo;
            var startTime = DateTime.Now;
            var serviceName = _calculator.GetType().Name;
            var methodName = methodInfo.Name;

            try
            {
                Console.WriteLine("调用{0}服务的{1}方法开始...", serviceName, methodName);
                var argsInfo = new Dictionary<string, object>();
                for (int i = 0; i < methodCall.ArgCount; i++)
                {
                    argsInfo.Add(methodCall.GetArgName(i), methodCall.Args[i]);
                }
                Console.WriteLine("当前传入参数:{0}", JsonConvert.SerializeObject(argsInfo));
                var result = methodInfo.Invoke(_calculator, methodCall.InArgs);
                if (result != null)
                    Console.WriteLine("当前返回值:{0}", JsonConvert.SerializeObject(result));
                return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
            }
            catch (Exception ex)
            {
                Console.WriteLine("调用{0}服务的{1}方法失败,失败原因：{2}", serviceName, methodName, ex.Message);
                throw ex;
            }
            finally
            {
                Console.WriteLine("调用{0}服务的{1}方法结束,共耗时{2}秒", serviceName, methodName, DateTime.Now.Subtract(startTime).TotalSeconds);
                Console.WriteLine("----------------------------------");
            }
        }
    }
}
