namespace StoreAdmin.Core.Stores
{
    public class Store
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? EmailAddress { get; set; }
        public string? Address { get; set; }
        public bool IsEnabled { get; set; }
    }
}