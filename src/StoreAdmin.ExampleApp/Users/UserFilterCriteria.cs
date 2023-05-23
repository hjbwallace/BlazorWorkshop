using StoreAdmin.Core.Users;

namespace StoreAdmin.ExampleApp.Users
{
    public class UserFilterCriteria
    {
        public string? Name { get; set; }
        public int StoreId { get; set; }

        public bool AppliesTo(User user)
        {
            return (user.Name?.Contains(Name ?? "", StringComparison.InvariantCultureIgnoreCase) ?? false)
                && (StoreId == 0 || user.StoreIds.Contains(StoreId));
        }
    }
}