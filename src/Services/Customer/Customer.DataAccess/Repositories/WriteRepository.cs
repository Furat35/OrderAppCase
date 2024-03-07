using Customer.DataAccess.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.Abstract;
using Shared.Entities.Common;

namespace Customer.DataAccess.Repositories
{
    public class WriteRepository<TEntity> : IWriteRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        private readonly EfCustomerContext _context;
        public WriteRepository(EfCustomerContext context)
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
