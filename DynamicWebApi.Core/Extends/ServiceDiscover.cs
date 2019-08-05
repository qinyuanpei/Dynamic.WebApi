using Consul;
using Grpc.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ServiceDiscover> _logger;

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
        public ServiceDiscover(ILogger<ServiceDiscover> logger, IConsulClient consul, IMemoryCache memoryCache)
        {
            _logger = logger;
            _consul = consul;
            _memoryCache = memoryCache; ;

        }

        public async Task<object> FindServiceAsync<TRpcServiceImp>() where TRpcServiceImp : class
        {
            var cacheKey = typeof(TRpcServiceImp).Name;
            var clientType = (typeof(TRpcServiceImp).GetCustomAttributes(typeof(GrpcServiceBindAttribute), false)[0] as GrpcServiceBindAttribute).ClientType;
            var serviceUrl = string.Empty;
            var clientInstance = new object();
            if (!_memoryCache.TryGetValue<object>(cacheKey, out clientInstance))
            {
                var services = await _consul.Health.Service(cacheKey, string.Empty, true);
                var serviceUrls = services.Response.Select(s => $"{s.Service.Address}:{s.Service.Port}").ToList();
                _logger.LogInformation($"There are {serviceUrls.Count} services found in consul");
                if (serviceUrls == null || !serviceUrls.Any())
                    throw new Exception($"Please make sure service {cacheKey} is registered in consul");

                serviceUrl = serviceUrls[new Random().Next(0, serviceUrls.Count - 1)];
                _logger.LogInformation($"{serviceUrl} is ready to use as a channel for gRpc");
                var channel = new Channel(serviceUrl, ChannelCredentials.Insecure);
                var constructorInfo = clientType.GetConstructor(new Type[] { typeof(Channel) });
                if (constructorInfo == null)
                    throw new Exception($"Please make sure {clientType.Name} is a gRpc client");

                clientInstance = constructorInfo.Invoke(new object[] { channel });
                _memoryCache.Set<object>(cacheKey, clientInstance, DateTimeOffset.UtcNow.AddMinutes(5));
                return Task.FromResult<object>(clientInstance);
            }
            else
            {
                _logger.LogInformation($"Using client instance for {clientType.Name} form caching...");
                return clientInstance;
            }
        }
    }
}
