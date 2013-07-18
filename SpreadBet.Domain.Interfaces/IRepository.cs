namespace SpreadBet.Domain.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IRepository
	{
        T Get<T>(Expression<Func<T, bool>> where) where T : Entity;
		IEnumerable<T> GetAll<T>() where T : Entity;
        IEnumerable<T> GetAll<T>(params Expression<Func<T, object>>[] includeProperties) where T : Entity;
        IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties) where T : Entity;
		void SaveOrUpdate<T>(T entity) where T : Entity;
		void Delete<T>(T entity) where T : Entity;
	}
}
