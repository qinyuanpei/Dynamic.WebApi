using Consul;
using Grpc.Core;
using Grpc.Core.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicWebApi.Core.Extends
{
    public class ServiceDiscover : IServiceDiscover
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Consul
        /// </summary>
        private readonly IConsulClient _consul;

        /// <summary>
        /// MemoryCache
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="consul">Consul</param>
        public ServiceDiscover(ILogger logger, IConsulClient consul, IMemoryCache memoryCache)
        {
            _logger = logger;
            _consul = consul;
            _memoryCache = memoryCache; ;

        }

        public async Task<object> FindServiceAsync<TRpcServiceImp>() where TRpcServiceImp : class
        {
            var cacheKey = typeof(TRpcServiceImp).Name;
            var serviceUrl = string.Empty;
            if (!_memoryCache.TryGetValue<string>(cacheKey, out serviceUrl))
            {
                var services = await _consul.Health.Service(cacheKey, string.Empty, true);
                var serviceUrls = services.Response.Select(s => $"{s.Service.Address}:{s.Service.Port}").ToList();
                serviceUrl = serviceUrls[new Random().Next(0, serviceUrls.Count - 1)];
                _memoryCache.Set<string>(cacheKey, serviceUrl, DateTimeOffset.UtcNow.AddMinutes(10));
            }

            //通过反射生成客户端
            var channel = new Channel(serviceUrl, ChannelCredentials.Insecure);
            var clientType = (typeof(TRpcServiceImp).GetCustomAttributes(typeof(GrpcServiceBindAttribute), false)[0] as GrpcServiceBindAttribute).ClientType;
            var constructorInfo = clientType.GetConstructor(new Type[] { typeof(Channel) });
            return Task.FromResult<object>(constructorInfo.Invoke(new object[] { channel }));
        }
    }
}
