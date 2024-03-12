using Microsoft.EntityFrameworkCore;
using Ordering.Persistence.Repositories.Context;
using Shared.DataAccess.Abstract;
using Shared.Entities.Common;

namespace Ordering.Persistence.Repositories
{
    public class WriteRepository<TEntity> : IWriteRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        private readonly EfOrderingContext _context;
        public WriteRepository(EfOrderingContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Table => _context.Set<TEntity>();

        public async Task<int> CreateAsync(TEntity model)
        {
            await Table.AddAsync(model);
            return await _context.SaveChangesAsync();
        }


        public async Task<int> DeleteAsync(TEntity model)
        {
            Table.Remove(model);
            return await _context.SaveChangesAsync();

        }

        public async Task<int> UpdateAsync(TEntity model)
        {
            Table.Update(model);
            return await _context.SaveChangesAsync();
        }
    }
}
