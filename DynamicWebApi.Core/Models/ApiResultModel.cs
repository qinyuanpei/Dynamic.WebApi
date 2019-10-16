using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicWebApi.Core.Models
{
    public class ApiResultModel<TResult>
    {
        public TResult Result { get; set; }
    }
}
