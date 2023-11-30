namespace Library.BusinessLogic.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetWhere(Func<T, bool> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
