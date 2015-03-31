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
        public List<NeighbourContinent> NeighbourContinents { get; set; }
        public List<Country> Countries { get; set; }

        public Continent(int ContinentID, string Name, List<NeighbourContinent> NeighbourContinents, List<Country> Countries)
        {
            this.ContinentID = ContinentID;
            this.Name = Name;
            this.NeighbourContinents = NeighbourContinents;
            this.Countries = Countries;
        }
    }
}