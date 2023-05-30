using Microsoft.Extensions.DependencyInjection;
using NSubstitute.ExceptionExtensions;
using StoreAdmin.Core.Data;
using StoreAdmin.Core.Stores;

namespace StoreAdmin.ExampleApp.Tests.Stores
{
    public class StoresPageTests : TestContext
    {
        private readonly IRepository<Store> _repository;

        public StoresPageTests()
        {
            _repository = Substitute.For<IRepository<Store>>();
            Services.AddSingleton(_repository);
        }

        [Fact]
        public void RendersWhenNoStoresFound()
        {
            HasStores();

            var component = RenderComponent<ExampleApp.Stores.Stores>();
            component.Find("#stores").MarkupMatches(@"<div id=""stores"">
    <p>No stores found</p>
</div>");
        }

        [Fact]
        public void RendersWhenStoresHaveError()
        {
            _repository
                .ListAsync()
                .ThrowsAsync(new Exception("FAILED!"));

            var component = RenderComponent<ExampleApp.Stores.Stores>();
            component.Find("#stores").MarkupMatches(@"<div id=""stores"">
    <p>Error: FAILED!</p>
</div>");
        }

        [Fact]
        public void RendersWhenStoresFound()
        {
            HasStores(new Store { Id = 1, Address = "123 Fake Street", EmailAddress = "email", IsEnabled = true, Name = "NAME" });

            var component = RenderComponent<ExampleApp.Stores.Stores>();

            component.Find("#stores").MarkupMatches(@"<div id=""stores"">
    <table class=""table"">
        <thead>
            <tr>
                <th>Id</th>
                <th>Name</th>
                <th>Email</th>
                <th>Address</th>
                <th>IsEnabled</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>1</td>
                <td>NAME</td>
                <td>email</td>
                <td>123 Fake Street</td>
                <td>True</td>
                <td><a href=""/stores/1"" class=""btn btn-secondary"">Edit</a></td>
            </tr>
        </tbody>
    </table>
</div>");
        }

        private void HasStores(params Store[] stores)
        {
            _repository
                .ListAsync()
                .Returns(Task.FromResult(stores));
        }
    }
}