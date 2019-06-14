using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Server.Service
{
    [ServiceContract]
    public interface ICalculator
    {
        [OperationContract]
        double Add(double n1, double n2);
        [OperationContract]
        double Subtract(double n1, double n2);
        [OperationContract]
        double Multiply(double n1, double n2);
        [OperationContract]
        double Divide(double n1, double n2);
    }

    public class CalculatorService : ICalculator
    {
        [HttpGet]
        public double Add(double n1, double n2)
        {
            return n1 + n2;
        }

        [HttpGet]
        public double Subtract(double n1, double n2)
        {
            return n1 - n2;
        }

        [HttpGet]
        public double Multiply(double n1, double n2)
        {
            return n1 * n2;
        }

        [HttpGet]
        public double Divide(double n1, double n2)
        {
            return n1 / n2;
        }
    }
}


