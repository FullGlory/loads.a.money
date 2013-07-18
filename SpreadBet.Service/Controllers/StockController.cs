using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpreadBet.Domain;
using SpreadBet.Domain.Interfaces;

namespace SpreadBet.Service.Controllers
{
    public class StockController : ApiController
    {
        private readonly IRepository _repository;

        public StockController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Stock/
        public IEnumerable<Stock> Get()
        {
            var list = _repository.GetAll<Stock>();
            if (list != null) return list;
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
        }

        // GET api/Stock/5
        public Stock Get(int id)
        {
            var item = _repository.Get<Stock>(s=>s.Id.Equals(id));
            if (item != null) return item;
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
