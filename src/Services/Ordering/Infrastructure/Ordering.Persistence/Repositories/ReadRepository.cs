﻿using Microsoft.EntityFrameworkCore;
using Ordering.Persistence.Repositories.Context;
using Shared.DataAccess.Abstract;
using Shared.Entities.Common;
using System.Linq.Expressions;

namespace Ordering.Persistence.Repositories
{
    public class ReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : class, IBaseEntity
    {
        private readonly EfOrderingContext _context;

        public ReadRepository(EfOrderingContext context)
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

        public IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate, bool tracking = false, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = SaveTrackingStatus(tracking);
            if (includeProperties.Any())
                foreach (var item in includeProperties)
                    query = query.Include(item);

            return query.Where(predicate);
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
