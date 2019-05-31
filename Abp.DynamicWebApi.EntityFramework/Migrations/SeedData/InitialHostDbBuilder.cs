using Abp.DynamicWebApi.EntityFramework;
using EntityFramework.DynamicFilters;

namespace Abp.DynamicWebApi.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly DynamicWebApiDbContext _context;

        public InitialHostDbBuilder(DynamicWebApiDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionsCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
        }
    }
}
