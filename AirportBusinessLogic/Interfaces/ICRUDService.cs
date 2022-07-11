
namespace AirportBusinessLogic.Interfaces
{
    public interface ICRUDService<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> Get(int id);
        Task<bool> Delete(int id);
        Task<bool> Update(T entity);
        void Create(T entity);
        Task SaveChangesAsync();
    
    }
}
