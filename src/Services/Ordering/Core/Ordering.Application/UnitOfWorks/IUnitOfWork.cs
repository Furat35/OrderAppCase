using Shared.DataAccess.Abstract;
using Shared.Entities.Common;

namespace Ordering.Application.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        IReadRepository<TEntity> GetReadRepository<TEntity>() where TEntity : class, IBaseEntity;
        IWriteRepository<TEntity> GetWriteRepository<TEntity>() where TEntity : class, IBaseEntity;
        Task<int> SaveAsync();
        int Save();
    }
}
