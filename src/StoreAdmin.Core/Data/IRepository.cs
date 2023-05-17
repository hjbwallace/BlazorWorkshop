namespace StoreAdmin.Core.Data
{
    public interface IRepository<T>
    {
        Task<T?> GetAsync(int id);

        Task<T[]> ListAsync();

        Task CreateAsync(T model);

        Task UpdateAsync(T model);
    }
}