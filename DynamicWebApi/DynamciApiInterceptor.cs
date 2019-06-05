using Castle.DynamicProxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;

namespace DynamicWebApi
{
    public class DynamciApiInterceptor<TService>: IInterceptor
    {
        private readonly TService _realObject;
        public DynamciApiInterceptor(TService realObject)
        {
            _realObject = realObject;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                var methodInfo = invocation.Method;
                var arguments = invocation.Arguments;
                var result = methodInfo.Invoke(_realObject, arguments);
                if (result != null)
                {
                    invocation.ReturnValue = result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
    }
}