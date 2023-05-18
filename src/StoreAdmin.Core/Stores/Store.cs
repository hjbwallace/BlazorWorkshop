using System.ComponentModel.DataAnnotations;

namespace StoreAdmin.Core.Stores
{
    public class Store
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? EmailAddress { get; set; }

        [Required]
        public string? Address { get; set; }

        public bool IsEnabled { get; set; }
    }
}