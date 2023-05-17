using StoreAdmin.Core.Users;

namespace StoreAdmin.Core.Stores
{
    public class StoreEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? EmailAddress { get; set; }
        public string? Address { get; set; }
        public bool IsEnabled { get; set; }
        public List<UserEntity> Users { get; set; } = new();

        public static Store ToStore(StoreEntity entity)
        {
            return new Store
            {
                Id = entity.Id,
                Address = entity.Address,
                EmailAddress = entity.EmailAddress,
                IsEnabled = entity.IsEnabled,
                Name = entity.Name,
            };
        }

        public static StoreEntity FromStore(Store store)
        {
            return new StoreEntity
            {
                Id = store.Id,
                Address = store.Address,
                EmailAddress = store.EmailAddress,
                IsEnabled = store.IsEnabled,
                Name = store.Name,
            };
        }
    }
}