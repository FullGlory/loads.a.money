using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using SpreadBet.Domain;

namespace SpreadBet.Repository
{
	public class EFRepository : IRepository, IDisposable
	{
		private Context _context;
		private readonly ConcurrentDictionary<Type, object> _dbSets =
			new ConcurrentDictionary<Type, object>();

        public EFRepository(Context context)
		{
			_context = context;
		}

        public T Get<T>(Expression<Func<T, bool>> where) where T : Entity
        {
            return GetDbSet<T>().Where(where).SingleOrDefault();
        }

		public IEnumerable<T> GetAll<T>() where T : Entity
		{
			return GetDbSet<T>().ToList();
		}

        public IEnumerable<T> GetAll<T>(params Expression<Func<T, object>>[] includeProperties) where T : Entity
        {
            IQueryable<T> queryable = GetDbSet<T>();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<T, object>(includeProperty);
            }
            return queryable.ToList();
        }

        public IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties) where T : Entity
        {
            IQueryable<T> queryable = GetDbSet<T>().Where(where);
            foreach (Expression<Func<T, object>> includeProperty in includeProperties) 
            { 
                queryable = queryable.Include<T, object>(includeProperty); 
            } 
            return queryable.ToList();
        }

        public void SaveOrUpdate<T>(T entity) where T : Entity
		{
			if (_context.Entry(entity).State == EntityState.Detached)
			{
				GetDbSet<T>().Add(entity);
			}
			else
			{
				_context.Entry(entity).State = EntityState.Modified;
			}

			_context.SaveChanges();
		}

		public void Delete<T>(T entity) where T : Entity
		{
			GetDbSet<T>().Remove(entity);
			_context.SaveChanges();
		}

		private DbSet<T> GetDbSet<T>() where T : Entity
		{
			return (DbSet<T>)_dbSets.GetOrAdd(typeof(T), x => _context.Set<T>());
		}

		public void Dispose()
		{
			if (_context != null)
			{
				_context.Dispose();
				_context = null;
			}
		}
    }
}
