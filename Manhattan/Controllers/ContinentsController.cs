﻿using Manhattan.Filters;
using Manhattan.Models;
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
        // GET continents
        [Route("1.0/continents", Name = "GetContinents")]
        public IEnumerable<Continent> Get()
        {
            return Continents.getContinents();
        }

        // GET continents/ID
        [Route("1.0/continents/{id}", Name = "GetContinent")]
        public Object Get(int id)
        {
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

        // POST continents
        [BasicAuthentication]
        [Route("1.0/continents", Name = "PostContinent")]
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

        // PUT continents/ID
        [BasicAuthentication]
        [Route("1.0/continents/{id}", Name = "PutContinent")]
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

        // DELETE continents/ID
        [BasicAuthentication]
        [Route("1.0/continents/{id}", Name = "DeleteContinent")]
        public object Delete(int id)
        {
            if (Continents.deleteContinent(id))
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