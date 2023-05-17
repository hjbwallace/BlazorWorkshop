using StoreAdmin.Core.Data;
using StoreAdmin.Core.Stores;
using StoreAdmin.Core.Users;

namespace StoreAdmin.Core
{
    public static class ExampleDataSeeder
    {
        public static async Task RunAsync(IRepository<Store> storeRepository, IRepository<User> userRepository)
        {
            var stores = Enumerable.Range(1, 20).Select(x => new Store
            {
                Name = $"Store {x}",
                EmailAddress = $"store{x}@example.com",
                Address = $"{x} Test Street, Perth, WA 6000",
                IsEnabled = true,
            });

            var users = Enumerable.Range(1, 10).Select(x => new User
            {
                IsEnabled = true,
                Name = $"User {x}",
                EmailAddress = $"user{x}@example.com",
                StoreIds = Enumerable.Range(1, 20).Where(e => e % x == 0).ToArray(),
            }).ToArray();

            foreach (var store in stores)
                await storeRepository.CreateAsync(store);

            foreach (var user in users)
                await userRepository.CreateAsync(user);
        }
    }
}