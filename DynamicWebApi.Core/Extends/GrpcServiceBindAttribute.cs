using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicWebApi.Core.Extends
{
    public class GrpcServiceBindAttribute : Attribute
    {
        private Type _bindType { get; set; }
        public GrpcServiceBindAttribute(Type bindType)
        {
            _bindType = bindType;
        }

        public Type BindType => _bindType;
    }
}
