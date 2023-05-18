using System.ComponentModel.DataAnnotations;

namespace StoreAdmin.Core.Users
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? EmailAddress { get; set; }

        public bool IsEnabled { get; set; }

        public int[] StoreIds { get; set; } = Array.Empty<int>();
    }
}