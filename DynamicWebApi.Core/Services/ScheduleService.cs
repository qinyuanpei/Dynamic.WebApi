using DynamicWebApi.Core.Extends;
using DynamicWebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicWebApi.Core.Services
{
    public class ScheduleService:IDynamicController
    {
        [HttpPost]
        public ApiResultModel<ScheduleModel> Add(ScheduleModel schedule)
        {
            return new ApiResultModel<ScheduleModel>() { Result = schedule };
        }
    }
}
