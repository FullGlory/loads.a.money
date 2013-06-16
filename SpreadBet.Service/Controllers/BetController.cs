using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpreadBet.Domain;
using SpreadBet.Repository;

namespace SpreadBet.Service.Controllers
{
    public class BetController : ApiController
    {
        private readonly IRepository _repository;

        public BetController(IRepository repository)
        {
            _repository = repository;
        }

        // GET api/bet
        public IEnumerable<Bet> Get()
        {
            var list = _repository.GetAll<Bet>();
            if (list != null) return list;
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
        }

        // GET api/bet/5
        public Bet Get(int id)
        {
            var item = _repository.Get<Bet>(b => b.Id.Equals(id));
            if (item != null) return item;
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}