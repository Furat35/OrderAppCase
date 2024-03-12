using Shared.Entities.Common;
using System.Linq.Expressions;

namespace Shared.DataAccess.Abstract
{
    public interface IReadRepository<TEntity>
       where TEntity : IBaseEntity
    {
        IQueryable<TEntity> Get(bool tracking = true, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetByIdAsync(Guid id, bool tracking = true, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate, bool tracking = false, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
