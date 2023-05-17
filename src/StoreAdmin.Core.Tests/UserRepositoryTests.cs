using Microsoft.EntityFrameworkCore;
using StoreAdmin.Core.Tests.Mocks;
using StoreAdmin.Core.Users;

namespace StoreAdmin.Core.Tests
{
    public class UserRepositoryTests
    {
        private readonly InMemoryContextFactory _contextFactory;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            _contextFactory = new InMemoryContextFactory();
            _repository = new UserRepository(_contextFactory);
        }

        [Fact]
        public async Task CanCreateUser()
        {
            await CreateUser("TEST USER");
            await AssertUserCount(1);
        }

        [Fact]
        public async Task CanUpdateUser()
        {
            var user = await CreateUser("TEST USER");
            user.Name = "UPDATED USER";

            await _repository.UpdateAsync(user);

            await AssertUser(user.Id, new { Name = "UPDATED USER" });
            await AssertUserCount(1);
        }

        [Fact]
        public async Task ThrowsWhenCreatingUserWithSetId()
        {
            var user = new User { Id = 1 };
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.CreateAsync(user));
            Assert.Equal("User cannot be created with a specific ID", ex.Message);
        }

        [Fact]
        public async Task ThrowsWhenUpdatingUserWithoutSetId()
        {
            var user = new User { Id = 0 };
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.UpdateAsync(user));
            Assert.Equal("UserID is not valid", ex.Message);
        }

        [Fact]
        public async Task ThrowsWhenUpdatingMissingUser()
        {
            var user = new User { Id = 1 };
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.UpdateAsync(user));
            Assert.Equal("No user found with a matching ID: 1", ex.Message);
        }

        private async Task<User> CreateUser(string name)
        {
            var user = new User { Name = name };
            await _repository.CreateAsync(user);

            return await GetUser(name);
        }

        private async Task<User> GetUser(string name)
        {
            var users = await _repository.ListAsync();
            return users.Single(x => x.Name == name);
        }

        private async Task AssertUserCount(int expected)
        {
            using var context = _contextFactory.CreateDbContext();
            var count = await context.Users.CountAsync();
            Assert.Equal(expected, count);
        }

        private async Task AssertUser(int id, object expected)
        {
            using var context = _contextFactory.CreateDbContext();
            var store = await context.Users.FindAsync(id);
            store.Should().BeEquivalentTo(expected);
        }
    }
}