using Microsoft.EntityFrameworkCore;
using StoreAdmin.Core.Stores;
using StoreAdmin.Core.Users;

namespace StoreAdmin.Core.Data
{
    public class WorkshopDbContext : DbContext
    {
        public WorkshopDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<StoreEntity> Stores { get; set; } = null!;
        public DbSet<UserEntity> Users { get; set; } = null!;
    }
}