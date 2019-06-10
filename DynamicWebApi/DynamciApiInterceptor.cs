using Castle.DynamicProxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Results;

namespace DynamicWebApi
{
    public class DynamicApiInterceptor<TService> : IInterceptor
    {
        private readonly TService _realObject;

        public DynamicApiInterceptor(TService realObject)
        {
            _realObject = realObject;
        }

        public void Intercept(IInvocation invocation)
        {
            if (typeof(TService).IsAssignableFrom(invocation.Method.DeclaringType))
            {
                try
                {
                    var result = invocation.Method.Invoke(_realObject, invocation.Arguments);
                    if (result != null)
                        invocation.ReturnValue = result;
                }
                catch (TargetInvocationException targetInvocation)
                {
                    if (targetInvocation.InnerException != null)
                        throw targetInvocation.InnerException;

                    throw; 
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}