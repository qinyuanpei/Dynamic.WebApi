using System.Linq;
using Abp.DynamicWebApi.EntityFramework;
using Abp.DynamicWebApi.MultiTenancy;

namespace Abp.DynamicWebApi.Migrations.SeedData
{
    public class DefaultTenantCreator
    {
        private readonly DynamicWebApiDbContext _context;

        public DefaultTenantCreator(DynamicWebApiDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUserAndRoles();
        }

        private void CreateUserAndRoles()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                _context.Tenants.Add(new Tenant {TenancyName = Tenant.DefaultTenantName, Name = Tenant.DefaultTenantName});
                _context.SaveChanges();
            }
        }
    }
}
