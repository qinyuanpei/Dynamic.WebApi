using DynamicWebApi.Core.Extends;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicWebApi.Core.Services
{
    [DynamicController]
    public class CoreCalculatorService : IDynamicController
    {
        private readonly ILogger _logger = Log.Logger;
      

        [HttpGet]
        public double Add(double n1, double n2)
        {
            _logger.Information($"Invoke {typeof(CoreCalculatorService).Name}/Add: {n1},{n2}");
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
