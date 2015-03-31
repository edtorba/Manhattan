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
        public List<int> NeighbourContinents { get; set; }
        public List<int> Countries { get; set; }

        public Continent(int ContinentID, string Name, List<int> NeighbourContinents, List<int> Countries)
        {
            this.ContinentID = ContinentID;
            this.Name = Name;
            this.NeighbourContinents = NeighbourContinents;
            this.Countries = Countries;
        }
    }
}