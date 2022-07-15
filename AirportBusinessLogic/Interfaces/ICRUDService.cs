
namespace AirportBusinessLogic.Interfaces
{
    public interface ICRUDService<T> where T : class
    {
        Task<List<T>> GetAll();
        T? Get(int id);
        Task Create(T entity);
        bool Update(T entity);


    }
}
