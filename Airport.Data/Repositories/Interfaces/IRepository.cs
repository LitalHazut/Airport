
namespace Airport.Data.Repositories.Interfaces
{
    public interface IRepository<T> where T: class
    {
        T Get(int id);
        IQueryable<T> GetAll();
        T Delete(int id);
        T Update(T entity);
        void Create(T entity);
    }
}
