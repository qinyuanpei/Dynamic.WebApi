using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicWebApi.Core.Extends
{
    interface IServiceDiscover
    {
        Task<object> FindServiceAsync<TRpcServiceImp>() where TRpcServiceImp : class;
    }
}
