using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using Abp.DynamicWebApi.Authorization.Roles;
using Abp.DynamicWebApi.Authorization.Users;
using Abp.DynamicWebApi.MultiTenancy;

namespace Abp.DynamicWebApi.EntityFramework
{
    public class DynamicWebApiDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public DynamicWebApiDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in DynamicWebApiDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of DynamicWebApiDbContext since ABP automatically handles it.
         */
        public DynamicWebApiDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public DynamicWebApiDbContext(DbConnection existingConnection)
         : base(existingConnection, false)
        {

        }

        public DynamicWebApiDbContext(DbConnection existingConnection, bool contextOwnsConnection)
         : base(existingConnection, contextOwnsConnection)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
