using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Manhattan.Models
{
    public class Continent
    {
        public int ContinentID { get; set; }
        public string Name { get; set; }
        public List<NeightbourContinent> NeightbourContinents { get; set; }
        public List<Country> Countries { get; set; }

        public Continent(int ContinentID, string Name, List<Country> Countries)
        {
            this.ContinentID = ContinentID;
            this.Name = Name;
            this.Countries = Countries;
        }
    }
}