﻿namespace Library.DataAccess.Repository
{
	public interface IRepository<T> where T : class
	{
		T Get(int id);
		IEnumerable<T> GetAll();
		IEnumerable<T> GetWhere(Func<T, bool> expression);
		void Create(T entity);
		void Update(T entity);
		void Delete(int id);
	}
}