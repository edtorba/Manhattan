using Manhattan.Models;
using Manhattan.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Manhattan.Controllers
{
    /**
     * ApiController Methods
     * https://msdn.microsoft.com/en-us/library/system.web.http.apicontroller_methods%28v=vs.118%29.aspx
     */
    public class CountriesController : ApiController
    {
        // GET api/countries
        public IEnumerable<Country> Get()
        {
            // Get list of countries
            return Countries.getCountries();
        }

        // GET api/countries/ID
        public Object Get(int id)
        {
            // Get country
            Object result = Countries.getCountry(id);
            if (result != null)
            {
                return result;
            }
            else
            {
                return this.NotFound();
            }
        }

        // POST api/countries
        public void Post([FromBody]Country country)
        {
            // TODO
        }

        // PUT api/countries/ID
        public void Put(int id, [FromBody]Country country)
        {
            // TODO
        }

        // DELETE api/countries/ID
        public void Delete(int id)
        {
            // TODO
        }
    }
}