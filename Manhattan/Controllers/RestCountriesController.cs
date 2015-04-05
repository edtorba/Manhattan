using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Manhattan.Controllers
{
    public class RestCountriesController : ApiController
    {
        public async Task<object> Get(string id)
        {
            return await RunAsync(id);
        }

        static async Task<object> RunAsync(string country)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://restcountries.eu/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("rest/v1/name/" + country);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<object>();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}