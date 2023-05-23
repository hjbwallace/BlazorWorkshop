namespace StoreAdmin.ExampleApp.Shared
{
    public class UiModel<T>
    {
        public UiModel(T entity)
        {
            Model = entity;
        }

        public T Model { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsSelected { get; set; } = true;
    }
}
