
namespace AirportBusinessLogic.Interfaces
{
    public interface ICRUDService<T> where T : class
    {
        List<T> GetAll();
        T? Get(int id);
        void Create(T entity);
        bool Update(T entity);
    }
}
