
namespace Airport.Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(int id);
        IQueryable<T> GetAll();
        Task Delete(int id);
        Task Update(T entity);
        Task Create(T entity);
        Task SaveChangesAsync();

    }
}
