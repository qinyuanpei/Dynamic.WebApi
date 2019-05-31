using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using Abp.DynamicWebApi.EntityFramework;

namespace Abp.DynamicWebApi
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(DynamicWebApiCoreModule))]
    public class DynamicWebApiDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DynamicWebApiDbContext>());

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
