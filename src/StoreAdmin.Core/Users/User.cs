namespace StoreAdmin.Core.Users
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? EmailAddress { get; set; }
        public bool IsEnabled { get; set; }
        public int[] StoreIds { get; set; } = Array.Empty<int>();
    }
}