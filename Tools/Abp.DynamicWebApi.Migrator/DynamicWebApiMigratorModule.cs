using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.DynamicWebApi.EntityFramework;

namespace Abp.DynamicWebApi.Migrator
{
    [DependsOn(typeof(DynamicWebApiDataModule))]
    public class DynamicWebApiMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<DynamicWebApiDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}