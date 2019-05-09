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

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var binding = new BasicHttpBinding();
            var serviceUrl = "http://localhost:8502/Calculator.svc";
            var calculator = ServiceProxyFactory.CreatePorxy<Server.Service.ICalculator>(binding, serviceUrl);
            for (int i = 0; i < 100; i++)
            {
                calculator.Add(12, 24);
                calculator.Subtract(36, 10);
                calculator.Multiply(12, 35);
                calculator.Divide(36, 12);
            }

            calculator = ServiceProxyFactory.CreateCastleProxy<Server.Service.ICalculator>(binding, serviceUrl);
            for(int i = 0; i < 100; i++)
            {
                calculator.Add(12, 24);
                calculator.Subtract(36, 10);
                calculator.Multiply(12, 35);
                calculator.Divide(36, 12);
            }


            serviceUrl = "http://localhost:8502/Message.svc";
            var message = ServiceProxyFactory.CreatePorxy<Server.Service.IMessage>(binding, serviceUrl);
            message.Echo("小古");



            Console.ReadKey();
        }
    }
}

