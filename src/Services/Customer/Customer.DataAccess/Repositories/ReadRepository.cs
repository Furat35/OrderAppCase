using Customer.DataAccess.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.Abstract;
using Shared.Entities.Common;
using System.Linq.Expressions;

namespace Customer.DataAccess.Repositories
{
    public class ReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : class, IBaseEntity
    {
        private readonly EfCustomerContext _context;

        public ReadRepository(EfCustomerContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Table => _context.Set<TEntity>();

        public IQueryable<TEntity> Get(bool tracking = false, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = SaveTrackingStatus(tracking);
            if (includeProperties.Any())
                foreach (var item in includeProperties)
                    query = query.Include(item);

            return query;
        }

        public async Task<TEntity> GetByIdAsync(Guid id, bool tracking = true, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = SaveTrackingStatus(tracking);
            if (includeProperties.Any())
                foreach (var item in includeProperties)
                    query = query.Include(item);

            return await query.FirstOrDefaultAsync(_ => _.Id == id);
        }

        private IQueryable<TEntity> SaveTrackingStatus(bool tracking)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();

            return query;
        }
    }
}
