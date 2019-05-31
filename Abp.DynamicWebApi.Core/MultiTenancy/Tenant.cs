using Abp.MultiTenancy;
using Abp.DynamicWebApi.Authorization.Users;

namespace Abp.DynamicWebApi.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {
            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}