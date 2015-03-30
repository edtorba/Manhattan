using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manhattan.Models
{
    public class Country
    {
        public int CountryID { get; set; }
        public string Name { get; set; }
        public string Capital { get; set; }

        public Country(int CountryID, string Name, string Capital)
        {
            this.CountryID = CountryID;
            this.Name = Name;
            this.Capital = Capital;
        }
    }
}