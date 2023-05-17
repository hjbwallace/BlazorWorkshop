using Microsoft.EntityFrameworkCore;
using StoreAdmin.Core.Data;

namespace StoreAdmin.Core.Stores
{
    public class StoreRepository : IRepository<Store>
    {
        private readonly IDbContextFactory<WorkshopDbContext> _contextFactory;

        public StoreRepository(IDbContextFactory<WorkshopDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Store?> GetAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var entity = await context.Stores
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity == null ? null : StoreEntity.ToStore(entity);
        }

        public async Task<Store[]> ListAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var entities = await context.Stores
                .AsNoTracking()
                .ToArrayAsync();

            return entities.Select(StoreEntity.ToStore).ToArray();
        }

        public async Task CreateAsync(Store store)
        {
            if (store.Id != default)
                throw new InvalidOperationException("Store cannot be created with a specific ID");

            var entity = StoreEntity.FromStore(store);

            using var context = await _contextFactory.CreateDbContextAsync();

            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Store store)
        {
            if (store.Id == default)
                throw new InvalidOperationException("StoreID is not valid");

            using var context = await _contextFactory.CreateDbContextAsync();
            var entity = await context.FindAsync<StoreEntity>(store.Id);

            if (entity == null)
                throw new InvalidOperationException("No store found with a matching ID: " + store.Id);

            entity.Address = store.Address;
            entity.EmailAddress = store.EmailAddress;
            entity.IsEnabled = store.IsEnabled;
            entity.Name = store.Name;

            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}