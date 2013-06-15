using System.Linq;
using System.Web.Http;
using SpreadBet.Repository;
using SpreadBet.Domain;
using System.Web.Security;
using System.Net.Http;
using System.Net;
using System;
using System.Collections.Generic;

namespace SpreadBet.Service.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IRepository _repository;

        public AccountController(IRepository repository)
        {
            _repository = repository;
        }

        // GET api/account
        public IEnumerable<Account> Get()
        {
            var list = _repository.GetAll<Account>();
            if (list != null) return list;
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
        }

        // GET api/account/5
        public Account Get(int id)
        {
            var item = _repository.Get<Account>(id);
            if (item != null) return item;
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
        }

        // POST api/account
        public HttpResponseMessage Post([FromBody] Account model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var account = _repository.GetAll<Account>().FirstOrDefault(x => x.Username == model.Username && model.Password == model.Password);
                    if (account != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Username, false);
                        var response = Request.CreateResponse<Account>(HttpStatusCode.Created, account);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Model state is invalid");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
    }
}
