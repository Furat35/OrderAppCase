using Customer.DataAccess.Repositories;
using Customer.DataAccess.Repositories.Context;
using Shared.DataAccess.Abstract;
using Shared.Entities.Common;

namespace Customer.DataAccess.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EfCustomerContext _context;

        public UnitOfWork(EfCustomerContext context)
        {
            _context = context;
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
