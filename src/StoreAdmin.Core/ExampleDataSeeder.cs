using StoreAdmin.Core.Data;
using StoreAdmin.Core.Stores;
using StoreAdmin.Core.Users;

namespace StoreAdmin.Core
{
    public static class ExampleDataSeeder
    {
        private static readonly (string Suburb, string Postcode)[] Locations = new[]
        {
            ("Fremantle", "6160"),
            ("Scarborough", "6019"),
            ("Subiaco", "6008"),
            ("Cottesloe", "6011"),
            ("Joondalup", "6027"),
            ("Victoria Park", "6100"),
            ("Mount Lawley", "6050"),
            ("Claremont", "6010"),
            ("Rockingham", "6168"),
            ("Armadale", "6112"),
            ("South Perth", "6151"),
            ("Leederville", "6007"),
            ("Wembley", "6014"),
            ("Maylands", "6051"),
            ("Karrinyup", "6018"),
        };

        private static readonly string[] Names = new[]
        {
            "Joe Bloggs",
            "John Smith",
            "Jane Smithson",
            "Danny Developer",
            "Emily Engineer",
            "Terry Tester",
            "Sandy Supervisor",
            "Izzy Intern",
            "Gregory Graduate",
            "Joey Junior",
            "Marty Manager",
        };

        public static async Task RunAsync(IRepository<Store> storeRepository, IRepository<User> userRepository)
        {
            var stores = Locations.Select((x, i) => new Store
            {
                Name = x.Suburb,
                EmailAddress = $"{x.Suburb.Replace(" ", "").ToLower()}@example.com",
                Address = $"{i + 1} Test Street, {x.Suburb}, WA {x.Postcode}",
                IsEnabled = true,
            }).ToArray();

            var users = Names.Select((x, i) => new User
            {
                Name = x,
                EmailAddress = $"{x.Replace(" ", "").ToLower()}@example.com",
                StoreIds = Enumerable.Range(1, Locations.Length).Where(e => e % (i + 1) == 0).ToArray(),
                IsEnabled = true,
            }).ToArray();

            foreach (var store in stores)
                await storeRepository.CreateAsync(store);

            foreach (var user in users)
                await userRepository.CreateAsync(user);
        }
    }
}