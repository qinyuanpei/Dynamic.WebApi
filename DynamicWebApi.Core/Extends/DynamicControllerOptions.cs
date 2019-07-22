using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicWebApi.Core.Extends
{
    public class DynamicControllerOptions
    {
        /// <summary>
        /// 是否优先使用自定义路由
        /// </summary>
        public bool UseCustomRouteFirst { get; set; } = true;

        /// <summary>
        /// 是否使用RESTful风格的Action
        /// </summary>
        public bool UseRestfulActionName { get; set; } = true;

        /// <summary>
        /// 是否使用友好的控制器名称
        /// </summary>
        public bool UseFriendlyControllerName { get; set; } = true;

        /// <summary>
        /// 默认Api路由名称前缀
        /// </summary>
        public string DefaultApiRoutePrefix { get; set; } = "api";

        /// <summary>
        /// 默认删除的控制器名称后缀
        /// </summary>
        public string[] RemoveControllerSuffix { get; set; } = new string[] { "Controller", "Service", "AppService" };

        /// <summary>
        /// 默认配置
        /// </summary>
        public static DynamicControllerOptions Default => new DynamicControllerOptions();
    }
}
