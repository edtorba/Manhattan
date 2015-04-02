using Manhattan.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Manhattan.Repository
{
    public class Countries
    {
        private static string _ConnectionString = WebConfigurationManager
                                                    .ConnectionStrings["SOFT338_ConnectionString"]
                                                    .ConnectionString;

        /**
         * Retrieve list of all countries
         * 
         * GET api/countries
         */
        public static List<Country> getCountries()
        {
            // Create a new list to store countries
            List<Country> countries = new List<Country>();

            // Establish a new connection using connection string in the web.config
            SqlConnection connection = new SqlConnection(_ConnectionString);

            /**
             * SQL object that stores SQL query
             *
             * 
                SELECT
                    Name
                FROM Country
             * 
             */
            SqlCommand selectCountriesSql = new SqlCommand("SELECT " +
                "CountryID, Name, Capital " +
            "FROM Country",
            connection);

            // Loop through data and push to List<Continent> continents
            using (connection)
            {
                // The connection is automatically closed at the end of the using block.
                connection.Open();

                // SQL reader
                SqlDataReader countriesList = selectCountriesSql.ExecuteReader();
                while (countriesList.Read())
                {
                    /**
                     * Country
                     */
                    if (countriesList["Name"] != DBNull.Value)
                    {
                        Country country = new Country(
                            (int)countriesList["CountryID"],
                            (string)countriesList["Name"],
                            (string)countriesList["Capital"]
                        );

                        // Check if country is already in countries list
                        if (!countries.Exists(x => x.CountryID.Equals(country.CountryID)))
                        {
                            // Add country to countries list
                            countries.Add(country);
                        }
                    }
                }
            }

            return countries;
        }
    }
}