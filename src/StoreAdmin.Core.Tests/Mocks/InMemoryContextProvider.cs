using Microsoft.EntityFrameworkCore;
using StoreAdmin.Core.Data;

namespace StoreAdmin.Core.Tests.Mocks
{
    public class InMemoryContextFactory : IDbContextFactory<WorkshopDbContext>
    {
        private readonly DbContextOptions<WorkshopDbContext> _options;

        public InMemoryContextFactory()
        {
            _options = new DbContextOptionsBuilder<WorkshopDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        public WorkshopDbContext CreateDbContext()
        {
            return new WorkshopDbContext(_options);
        }
    }
}