namespace Library.DataAccess.Repository
{
    using Library.DataAccess.Data;
    using Library.DataAccess.Repository.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public abstract class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		private readonly DbSet<T> _dbSet;
		
		public Repository(ApplicationDbContext db)
		{
			_db= db;
			_dbSet = db.Set<T>();
		}

		public virtual T Get(int id)
		{
			return _dbSet.Find(id);
		}

		public virtual IEnumerable<T> GetAll()
		{
			return _dbSet.ToList();
		}

		public virtual IEnumerable<T> GetWhere(Func<T, bool> expression)
		{
			return _dbSet.Where(expression).ToList();
		}

		public void Create(T entity)
		{
			_dbSet.Add(entity);
			_db.SaveChanges();
		}

		public void Update(T entity)
		{
			_dbSet.Update(entity);
			_db.SaveChanges();
		}

		public void Delete(int id)
		{
			var entity = _dbSet.Find(id);
			if (entity != null)
			{
				_dbSet.Remove(entity);
				_db.SaveChanges();
			}
		}
	}
}
