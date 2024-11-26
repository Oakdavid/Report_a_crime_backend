namespace Report_A_Crime.Models.Repositories.Interface
{
    public interface IBaseRepository<T>
    {
        Task<T> CreateAsync(T entity);
        public Task<int> SaveAsync();
        public T Update (T entity); 
    }
}
