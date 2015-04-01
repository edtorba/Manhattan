using Manhattan.Models;
using Manhattan.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Manhattan.Controllers
{
    public class ContinentsController : ApiController
    {
        // GET api/continents
        public IEnumerable<Continent> Get()
        {
            // Get list of continents
            return Continents.getContinents();
        }

        // Get api/continents/ID
        public Object Get(int id)
        {
            // Get continent
            return Continents.getContinent(id);
        }

        // POST api/continents
        public void Post([FromBody]string value)
        {
            // TODO
        }

        // PUT api/continents/ID
        public void Put(int id, [FromBody]string value)
        {
            // TODO Insert new
        }

        // DELETE api/continents/ID
        public void Delete(int id)
        {
            // TODO detele
        }
    }
}