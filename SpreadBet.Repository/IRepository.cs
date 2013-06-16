using SpreadBet.Domain;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;

namespace SpreadBet.Repository
{
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
