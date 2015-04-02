﻿using Manhattan.Models;
using Manhattan.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Manhattan.Controllers
{
    /**
     * ApiController Methods
     * https://msdn.microsoft.com/en-us/library/system.web.http.apicontroller_methods%28v=vs.118%29.aspx
     */
    public class ContinentsController : ApiController
    {
        // GET api/continents
        public IEnumerable<Continent> Get()
        {
            // Get list of continents
            return Continents.getContinents();
        }

        // GET api/continents/ID
        public Object Get(int id)
        {
            // Get continent
            Object result = Continents.getContinent(id);
            if (result != null)
            {
                return result;
            }
            else
            {
                return this.NotFound();
            }
        }

        // POST api/continents
        public object Post([FromBody]Continent continent)
        {
            if (continent != null)
            {
                if (Continents.postContinent(continent))
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

        // PUT api/continents/ID
        public object Put(int id, [FromBody]Continent continent)
        {
            if (continent != null)
            {
                if (Continents.putContinent(id, continent))
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

        // DELETE api/continents/ID
        public void Delete(int id)
        {
            // Delete continent
            if (Continents.deleteContinent(id))
            {
                this.StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                this.NotFound();
            }
        }
    }
}