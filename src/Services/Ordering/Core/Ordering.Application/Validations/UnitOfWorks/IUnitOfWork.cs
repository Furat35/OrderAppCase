using Shared.DataAccess.Abstract;
using Shared.Entities.Common;

namespace Ordering.Application.Validations.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        IReadRepository<TEntity> GetReadRepository<TEntity>() where TEntity : class, IBaseEntity;
        IWriteRepository<TEntity> GetWriteRepository<TEntity>() where TEntity : class, IBaseEntity;
        Task<int> SaveAsync();
        int Save();
    }
}
