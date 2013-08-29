namespace SpreadBet.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using SpreadBet.Domain;
    using SpreadBet.Domain.Interfaces;

    public class EFRepository : IRepository
	{
        public T Get<T>(Expression<Func<T, bool>> where) where T : Entity
        {
            using (var ctx = new Context())
            {
                return ctx.Set<T>().Where(where).SingleOrDefault();
            }
        }

		public IEnumerable<T> GetAll<T>() where T : Entity
		{
            using (var ctx = new Context())
            {
                return ctx.Set<T>().ToList();
            }
		}

        public IEnumerable<T> GetAll<T>(params Expression<Func<T, object>>[] includeProperties) where T : Entity
        {
            using (var ctx = new Context())
            {
                IQueryable<T> queryable = ctx.Set<T>();
                foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                {
                    queryable = queryable.Include<T, object>(includeProperty);
                }
                return queryable.ToList();
            }
        }

        public IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties) where T : Entity
        {
            using (var ctx = new Context())
            {
                IQueryable<T> queryable = ctx.Set<T>().Where(where);
                foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                {
                    queryable = queryable.Include<T, object>(includeProperty);
                }
                return queryable.ToList();
            }
        }

        public void SaveOrUpdate<T>(T entity) where T : Entity
		{
            using (var ctx = new Context())
            {
              RecurseObjectGraph(ctx, entity);

              ctx.SaveChanges();
            }
		}

        /// <summary>
        /// Recurses the object graph - attaching any entity to the EF Context if it is "known"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctx">The CTX.</param>
        /// <param name="entity">The entity.</param>
        private void RecurseObjectGraph<T>(Context ctx, T entity) where T : Entity
        {
            // Check children
            var entityType = typeof(Entity);
            foreach (var prop in entity.GetType().GetProperties())
            {
                if (entityType.IsAssignableFrom(prop.PropertyType))
                {
                    object propValue = prop.GetValue(entity, null);
                    if (propValue != null)
                    {
                        RecurseObjectGraph(ctx, (Entity)propValue);
                    }
                }
            }

            // Check parent
            ctx.Entry(entity).State = (entity.Id == 0) ? EntityState.Added : EntityState.Modified;
        }

		public void Delete<T>(T entity) where T : Entity
		{
            using (var ctx = new Context())
            {
                ctx.Set<T>().Remove(entity);
                ctx.SaveChanges();
            }
		}
    }
}
