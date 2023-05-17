using Microsoft.EntityFrameworkCore;
using StoreAdmin.Core.Stores;
using StoreAdmin.Core.Tests.Mocks;

namespace StoreAdmin.Core.Tests
{
    public class StoreRepositoryTests
    {
        private readonly InMemoryContextFactory _contextFactory;
        private readonly StoreRepository _repository;

        public StoreRepositoryTests()
        {
            _contextFactory = new InMemoryContextFactory();
            _repository = new StoreRepository(_contextFactory);
        }

        [Fact]
        public async Task CanCreateStore()
        {
            await CreateStore("TEST STORE");
            await AssertStoreCount(1);
        }

        [Fact]
        public async Task CanUpdateStore()
        {
            var store = await CreateStore("TEST STORE");
            store.Name = "UPDATED STORE";

            await _repository.UpdateAsync(store);

            await AssertStore(store.Id, new { Name = "UPDATED STORE" });
            await AssertStoreCount(1);
        }

        [Fact]
        public async Task ThrowsWhenCreatingStoreWithSetId()
        {
            var store = new Store { Id = 1 };
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.CreateAsync(store));
            Assert.Equal("Store cannot be created with a specific ID", ex.Message);
        }

        [Fact]
        public async Task ThrowsWhenUpdatingStoreWithoutSetId()
        {
            var store = new Store { Id = 0 };
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.UpdateAsync(store));
            Assert.Equal("StoreID is not valid", ex.Message);
        }

        [Fact]
        public async Task ThrowsWhenUpdatingMissingStore()
        {
            var store = new Store { Id = 1 };
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.UpdateAsync(store));
            Assert.Equal("No store found with a matching ID: 1", ex.Message);
        }

        private async Task<Store> CreateStore(string name)
        {
            var store = new Store { Name = name };
            await _repository.CreateAsync(store);

            return await GetStore(name);
        }

        private async Task<Store> GetStore(string name)
        {
            var stores = await _repository.ListAsync();
            return stores.Single(x => x.Name == name);
        }

        private async Task AssertStoreCount(int expected)
        {
            using var context = _contextFactory.CreateDbContext();
            var count = await context.Stores.CountAsync();
            Assert.Equal(expected, count);
        }

        private async Task AssertStore(int id, object expected)
        {
            using var context = _contextFactory.CreateDbContext();
            var store = await context.Stores.FindAsync(id);
            store.Should().BeEquivalentTo(expected);
        }
    }
}