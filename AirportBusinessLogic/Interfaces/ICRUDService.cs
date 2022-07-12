
namespace AirportBusinessLogic.Interfaces
{
    public interface ICRUDService<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> Get(int id);
        Task Create(T entity);
        Task<bool> Update(T entity);


    }
}
