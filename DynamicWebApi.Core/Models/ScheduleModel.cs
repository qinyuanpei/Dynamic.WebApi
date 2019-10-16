using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicWebApi.Core.Models
{
    public class ScheduleModel
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string EventName { get; set; }

        public string Content { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
    }
}
