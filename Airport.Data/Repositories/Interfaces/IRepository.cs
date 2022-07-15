
namespace Airport.Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T? Get(int id);
        IQueryable<T> GetAll();
        bool Delete(int id);
        bool Update(T entity);
        void Create(T entity);
        Task SaveChangesAsync();

    }
}
