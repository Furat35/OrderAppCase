using Shared.Entities.Common;

namespace Shared.DataAccess.Abstract
{
    public interface IWriteRepository<TEntity>
          where TEntity : IBaseEntity
    {
        Task<int> CreateAsync(TEntity model);
        Task<int> DeleteAsync(TEntity model);
        Task<int> UpdateAsync(TEntity model);
    }
}
