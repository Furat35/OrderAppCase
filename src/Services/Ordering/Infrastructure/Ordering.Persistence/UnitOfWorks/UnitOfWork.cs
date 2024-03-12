using Ordering.Application.Validations.UnitOfWorks;
using Ordering.Persistence.Repositories;
using Ordering.Persistence.Repositories.Context;
using Shared.DataAccess.Abstract;
using Shared.Entities.Common;

namespace Ordering.Persistence.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EfOrderingContext _context;

        public UnitOfWork(EfOrderingContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async ValueTask DisposeAsync()
            => await _context.DisposeAsync();

        public IReadRepository<TEntity> GetReadRepository<TEntity>() where TEntity : class, IBaseEntity
            => new ReadRepository<TEntity>(_context);

        public IWriteRepository<TEntity> GetWriteRepository<TEntity>() where TEntity : class, IBaseEntity
            => new WriteRepository<TEntity>(_context);

        public int Save()
            => _context.SaveChanges();

        public async Task<int> SaveAsync()
            => await _context.SaveChangesAsync();
    }
}
