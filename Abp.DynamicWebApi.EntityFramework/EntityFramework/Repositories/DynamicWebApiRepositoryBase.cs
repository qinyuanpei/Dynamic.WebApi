using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace Abp.DynamicWebApi.EntityFramework.Repositories
{
    public abstract class DynamicWebApiRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<DynamicWebApiDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected DynamicWebApiRepositoryBase(IDbContextProvider<DynamicWebApiDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class DynamicWebApiRepositoryBase<TEntity> : DynamicWebApiRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected DynamicWebApiRepositoryBase(IDbContextProvider<DynamicWebApiDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
