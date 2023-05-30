using StoreAdmin.Core.Stores;
using StoreAdmin.ExampleApp.Stores;

namespace StoreAdmin.ExampleApp.Tests.Stores
{
    public class StoreFormTests : TestContext
    {
        private Store? _formStore;

        [Fact]
        public void CanSubmitFormWithValidStore()
        {
            var store = GetValidStore();
            var component = RenderForm();

            SubmitForm(component, store);

            HasSubmittedStore(store);
            HasNoValidationErrors(component);
        }

        [Fact]
        public void FormIsNotSubmittedWhenModelIsInvalid()
        {
            var component = RenderForm();

            SubmitForm(component, new Store());

            HasNotSubmittedStore();
            HasValidationErrors(component,
                "The Name field is required.",
                "The EmailAddress field is required.",
                "The Address field is required.");
        }

        [Fact]
        public void FormIsNotSubmittedWhenModelHasInvalidEmail()
        {
            var component = RenderForm();

            var store = GetValidStore();
            store.EmailAddress = "test";
            SubmitForm(component, store);

            HasNotSubmittedStore();
            HasValidationErrors(component, "The EmailAddress field is not a valid e-mail address.");
        }

        private static void SubmitForm(IRenderedComponent<StoreForm> component, Store store)
        {
            component.Find("input[id=name]").Change(store.Name);
            component.Find("input[id=email]").Change(store.EmailAddress);
            component.Find("input[id=address]").Change(store.Address);
            component.Find("input[id=isEnabled]").Change(store.IsEnabled);
            component.Find("button[type=submit]").Click();
        }

        private static void HasValidationErrors(IRenderedComponent<StoreForm> component, params string[] errors)
        {
            var validationErrorsElement = component!.FindAll(".validation-errors").Single();

            validationErrorsElement.MarkupMatches($@"<ul class=""validation-errors"">
    {string.Join(Environment.NewLine, errors.Select(e => $"<li class=\"validation-message\">{e}</li>"))}
</ul>");
        }

        private static void HasNoValidationErrors(IRenderedComponent<StoreForm> component)
        {
            var validationErrors = component!.FindAll(".validation-errors");
            validationErrors.Should().BeEmpty();
        }

        private static Store GetValidStore()
        {
            return new Store
            {
                Name = "Store",
                EmailAddress = "email@test.com",
                Address = "123 Fake Street",
                IsEnabled = true,
            };
        }

        private IRenderedComponent<StoreForm> RenderForm()
        {
            return RenderComponent<StoreForm>(parameters => parameters
                .Add(x => x.Store, new Store())
                .Add(x => x.OnSubmit, formStore => _formStore = formStore));
        }

        private void HasSubmittedStore(Store store)
        {
            _formStore.Should().BeEquivalentTo(store);
        }

        private void HasNotSubmittedStore()
        {
            _formStore.Should().BeNull();
        }
    }
}