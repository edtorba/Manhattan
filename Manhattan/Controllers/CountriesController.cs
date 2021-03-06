﻿using Manhattan.Filters;
using Manhattan.Models;
using Manhattan.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        // GET countries
        [Route("1.0/countries", Name = "GetCountries")]
        public IEnumerable<Country> Get()
        {
            return Countries.getCountries();
        }

        // GET countries/ID
        [Route("1.0/countries/{id}", Name = "GetCountry")]
        public Object Get(int id)
        {
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

        // POST countries
        [BasicAuthentication]
        [Route("1.0/countries", Name = "PostCountry")]
        public Object Post([FromBody]Country country)
        {
            if (country != null)
            {
                if (Countries.postCountry(country))
                {
                    return this.StatusCode(HttpStatusCode.Created);
                }
                else
                {
                    return this.BadRequest();
                }
            }
            else
            {
                return this.BadRequest();
            }
        }

        // PUT countries/ID
        [BasicAuthentication]
        [Route("1.0/countries/{id}", Name = "PutCountry")]
        public object Put(int id, [FromBody]Country country)
        {
            if (country != null)
            {
                if (Countries.putCountry(id, country))
                {
                    return this.StatusCode(HttpStatusCode.NoContent);
                }
                else
                {
                    return this.BadRequest();
                }
            }
            else
            {
                return this.BadRequest();
            }
        }

        // DELETE countries/ID
        [BasicAuthentication]
        [Route("1.0/countries/{id}", Name = "DeleteCountry")]
        public object Delete(int id)
        {
            if (Countries.deleteCountry(id))
            {
                return this.StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}