﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Manhattan.Controllers
{
    public class ContinentsController : ApiController
    {
        // GET api/continents
        public IEnumerable<string> Get()
        {
            return new string[] { "Europe", "Asia" };
        }

        // Get api/continents/ID
        public string Get(int id)
        {
            return "Europe";
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