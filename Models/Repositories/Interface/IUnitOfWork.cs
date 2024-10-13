namespace Report_A_Crime.Models.Repositories.Interface
{
    public interface IUnitOfWork
    {
        public Task<int> SaveChangesAsync();
    }
}
