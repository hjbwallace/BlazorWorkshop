using Microsoft.EntityFrameworkCore;
using StoreAdmin.Core.Data;
using StoreAdmin.Core.Stores;

namespace StoreAdmin.Core.Users
{
    public class UserRepository : IRepository<User>
    {
        private readonly IDbContextFactory<WorkshopDbContext> _contextFactory;

        public UserRepository(IDbContextFactory<WorkshopDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<User?> GetAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var entity = await context.Users
                .AsNoTracking()
                .Include(x => x.Stores)
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity == null ? null : UserEntity.ToUser(entity);
        }

        public async Task<User[]> ListAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var entities = await context.Users
                .AsNoTracking()
                .Include(x => x.Stores)
                .ToArrayAsync();

            return entities.Select(UserEntity.ToUser).ToArray();
        }

        public async Task CreateAsync(User user)
        {
            if (user.Id != default)
                throw new InvalidOperationException("User cannot be created with a specific ID");

            var entity = UserEntity.FromUser(user);

            using var context = await _contextFactory.CreateDbContextAsync();

            foreach (var store in entity.Stores)
                context.Entry(store).State = EntityState.Unchanged;

            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            if (user.Id == default)
                throw new InvalidOperationException("UserID is not valid");

            using var context = await _contextFactory.CreateDbContextAsync();
            var entity = await context.Users
                .Include(x => x.Stores)
                .FirstOrDefaultAsync(x => x.Id == user.Id);

            if (entity == null)
                throw new InvalidOperationException("No user found with a matching ID: " + user.Id);

            entity.EmailAddress = user.EmailAddress;
            entity.IsEnabled = user.IsEnabled;
            entity.Name = user.Name;

            var addedStoreIds = user.StoreIds.Where(x => !entity.Stores.Any(e => e.Id == x)).ToArray();
            var removedStores = entity.Stores.Where(x => !user.StoreIds.Any(e => e == x.Id)).ToArray();

            foreach (var store in removedStores)
                entity.Stores.Remove(store);

            foreach (var storeId in addedStoreIds)
                entity.Stores.Add(new StoreEntity { Id = storeId });

            foreach (var store in entity.Stores)
                context.Entry(store).State = EntityState.Unchanged;

            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}