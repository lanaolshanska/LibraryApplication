using Library.BusinessLogic.Interfaces;
using Library.DataAccess.Repository.Interfaces;

namespace Library.BusinessLogic
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private IRepository<T> _repository;

        public BaseService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public void Create(T entity)
        {
            _repository.Create(entity);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<T> GetWhere(Func<T, bool> expression)
        {
            return _repository.GetWhere(expression);
        }

        public void Update(T entity)
        {
            _repository.Update(entity);
        }
    }
}
