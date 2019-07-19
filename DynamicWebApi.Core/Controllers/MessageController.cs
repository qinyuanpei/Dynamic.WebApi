using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicWebApi.Core.Controllers
{
    public class MessageController
    {
        public string Echo(string receiver)
        {
            return $"Hello, {receiver}";
        }
    }
}
