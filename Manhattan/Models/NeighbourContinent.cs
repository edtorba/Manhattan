using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manhattan.Models
{
    public class NeighbourContinent
    {
        public int ContinentID { get; set; }
        public string Name { get; set; }

        public NeighbourContinent(int ContinentID, string Name)
        {
            this.ContinentID = ContinentID;
            this.Name = Name;
        }
    }
}