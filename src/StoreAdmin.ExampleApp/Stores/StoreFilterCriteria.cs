using StoreAdmin.Core.Stores;

namespace StoreAdmin.ExampleApp.Stores
{
    public class StoreFilterCriteria
    {
        public string? Name { get; set; }

        public bool AppliesTo(Store store)
        {
            return store.Name?.Contains(Name ?? "", StringComparison.InvariantCultureIgnoreCase) ?? false;
        }
    }
}
