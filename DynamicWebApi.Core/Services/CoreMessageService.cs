using DynamicWebApi.Core.Extends;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicWebApi.Core.Services
{
    public class CoreMessageService : IDynamicController
    {
        [HttpGet]
        public string Echo(string receiver)
        {
            return $"Hello, {receiver}";
        }
    }
}
