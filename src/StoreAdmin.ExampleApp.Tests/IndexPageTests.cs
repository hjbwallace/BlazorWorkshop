namespace StoreAdmin.ExampleApp.Tests
{
    public class IndexPageTests : TestContext
    {
        [Fact]
        public void IndexPageRendersCorrectly()
        {
            var component = RenderComponent<Pages.Index>();

            component.MarkupMatches(@"<h1>Store Admin</h1>
Welcome to the store admin site.");
        }
    }
}