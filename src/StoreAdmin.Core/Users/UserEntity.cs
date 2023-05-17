using StoreAdmin.Core.Stores;

namespace StoreAdmin.Core.Users
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? EmailAddress { get; set; }
        public bool IsEnabled { get; set; }
        public List<StoreEntity> Stores { get; set; } = new();

        public static User ToUser(UserEntity entity)
        {
            return new User
            {
                Id = entity.Id,
                EmailAddress = entity.EmailAddress,
                IsEnabled = entity.IsEnabled,
                Name = entity.Name,
                StoreIds = entity.Stores.Select(x => x.Id).ToArray(),
            };
        }

        public static UserEntity FromUser(User user)
        {
            return new UserEntity
            {
                Id = user.Id,
                EmailAddress = user.EmailAddress,
                IsEnabled = user.IsEnabled,
                Name = user.Name,
                Stores = user.StoreIds.Select(x => new StoreEntity { Id = x }).ToList(),
            };
        }
    }
}