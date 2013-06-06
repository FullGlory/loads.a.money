using SpreadBet.Domain;
using System.Linq;

namespace SpreadBet.Repository
{
	public interface IRepository
	{
		T Get<T>(int id) where T : Entity;
		IQueryable<T> GetAll<T>() where T : Entity;
		void SaveOrUpdate<T>(T entity) where T : Entity;
		void Delete<T>(T entity) where T : Entity;
	}
}
