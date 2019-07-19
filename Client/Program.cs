using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.ServiceModel;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.Reflection;
using Server.Service;
using Castle.Core.Interceptor;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.DynamicProxy;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //var binding = new BasicHttpBinding();
            //var serviceUrl = "http://localhost:8502/Calculator.svc";
            //var calculator = ServiceProxyFactory.CreatePorxy<Server.Service.ICalculator>(binding, serviceUrl);
            //for (int i = 0; i < 100; i++)
            //{
            //    //calculator.Add(12, 24);
            //    //calculator.Subtract(36, 10);
            //    //calculator.Multiply(12, 35);
            //    //calculator.Divide(36, 12);
            //}

            ////calculator = ServiceProxyFactory.CreateCastleProxy<Server.Service.ICalculator>(binding, serviceUrl);
            ////for (int i = 0; i < 100; i++)
            ////{
            ////    calculator.Add(12, 24);
            ////    calculator.Subtract(36, 10);
            ////    calculator.Multiply(12, 35);
            ////    calculator.Divide(36, 12);
            ////}


            //serviceUrl = "http://localhost:8502/Message.svc";
            //var message = ServiceProxyFactory.CreatePorxy<Server.Service.IMessage>(binding, serviceUrl);
            ////message.Echo("小古");



            ////var serviceProxy = new CalculatorServiceProxy(new CalculatorService());
            ////var calculator = (ICalculator)serviceProxy.GetTransparentProxy();
            ////calculator.Add(12, 24);
            ////calculator.Subtract(36, 10);
            ////calculator.Multiply(12, 35);
            ////calculator.Divide(36, 12);
            //Console.ReadKey();

            var container = new WindsorContainer();
            container.Register(
                Component.For<EchoService, IEchoService>(),
                Component.For(typeof(EchoInterceptor)).LifestyleTransient(),
                Component.For(typeof(EmptyClass)).Proxy.AdditionalInterfaces(typeof(IEchoService))
                    .Interceptors(typeof(EchoInterceptor)).LifestyleTransient()
            );

            var emptyClass = new EchoService();
            var methodInfo = emptyClass.GetType().GetMethod("Echo");
            methodInfo.Invoke(emptyClass, new object[] { "Dynamic WebApi" });
            Console.ReadKey();
        }

        #region 组合类和接口

        /// <summary>
        /// IEchoService定义
        /// </summary>
        public interface IEchoService
        {
            void Echo(string receiver);
        }

        /// <summary>
        /// IEchoServicee实现
        /// </summary>
        public class EchoService : IEchoService
        {
            public void Echo(string receiver)
            {
                Console.WriteLine($"Hi，{receiver}");
            }
        }

        /// <summary>
        /// 空类EmptyClass
        /// </summary>
        public class EmptyClass { }

        public class EchoInterceptor : IInterceptor
        {
            private IEchoService _realObject;
            public EchoInterceptor(IEchoService realObject)
            {
                _realObject = realObject;
            }

            public void Intercept(IInvocation invocation)
            {
                invocation.Method.Invoke(_realObject, invocation.Arguments);
            }
        }


        #endregion
    }
}

