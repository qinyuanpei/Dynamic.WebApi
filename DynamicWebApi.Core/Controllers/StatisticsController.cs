using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSRedis;

namespace DynamicWebApi.Core.Controllers
{
    public class StatisticsController: Controller
    {
        private readonly ILogger<MessageController> _logger;
        public StatisticsController(ILogger<MessageController> logger)
        {
            _logger = logger;
        }

        public ActionResult Analyze()
        {
            return Ok();
        }
    }
}
