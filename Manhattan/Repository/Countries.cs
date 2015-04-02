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

        /**
         * Retrieve specific country
         *
         * GET api/countries/ID
         */
        public static Country getCountry(int id)
        {
            // Initialise country object
            Country country = null;

            // Establish a new connection using connection string in the web.config
            SqlConnection connection = new SqlConnection(_ConnectionString);

            /**
             * SQL object that stores SQL query
             *
             * 
                SELECT
                    Name
                FROM Country
                WHERE CountryID = @id
             * 
             */
            SqlCommand selectCountrySql = new SqlCommand(null, connection);

            // Prepare statement
            selectCountrySql.CommandText = "SELECT " +
                "CountryID, Name, Capital " +
            "FROM Country " +
            "WHERE CountryID = @countryID";

            selectCountrySql.Parameters.AddWithValue("@countryID", id);

            // Loop through data and push to List<Continent> continents
            using (connection)
            {
                // The connection is automatically closed at the end of the using block.
                connection.Open();

                // SQL reader
                SqlDataReader countryData = selectCountrySql.ExecuteReader();
                while (countryData.Read())
                {
                    /**
                     * Country
                     */
                    if (country == null)
                    {
                        country = new Country(
                            (int)countryData["CountryID"],
                            (string)countryData["Name"],
                            (string)countryData["Capital"]
                        );
                    }
                }
            }

            return country;
        }
        
        /**
         * Insert new country
         * 
         * POST api/countries
         */
        public static Boolean postCountry(Country country)
        {
            // Validate object
            if (country.Name == null || country.Capital == null)
            {
                return false;
            }

            // Establish a new connection using connection string in the web.config
            SqlConnection connection = new SqlConnection(_ConnectionString);

            /**
             * SQL object that stores SQL query
             *
             *
                INSERT INTO Country
                    (Name, Capital)
                VALUES
                    (@name, @capital);
             * 
             */
            SqlCommand postCountrySql = new SqlCommand(null, connection);

            // Prepare statement for continent
            postCountrySql.CommandText = "INSERT INTO Country " +
                "(Name, Capital) " +
            "VALUES " +
                "(@name, @capital)";

            postCountrySql.Parameters.AddWithValue("@name", country.Name);
            postCountrySql.Parameters.AddWithValue("@capital", country.Capital);

            // Execute query
            using (connection)
            {
                // The connection is automatically closed at the end of the using block.
                connection.Open();

                // Executes insert query and returns identity of the most recently added record
                int affectedRows = (int)postCountrySql.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /**
         * Update specific country
         * 
         * PUT api/countries
         */
        public static Boolean putCountry(int id, Country country)
        {
            // Validate object
            if (country.Name == null || country.Capital == null)
            {
                return false;
            }

            // Establish a new connection using connection string in the web.config
            SqlConnection connection = new SqlConnection(_ConnectionString);

            /**
             * SQL object that stores SQL query
             *
             *
                UPDATE Country SET 
                    Name = @name, Capital = @capital
                WHERE CountryID = @countryID
             * 
             */
            SqlCommand putCountrySql = new SqlCommand(null, connection);

            // Prepare statement for continent
            putCountrySql.CommandText = "UPDATE Country SET  " +
                "Name = @name, Capital = @capital " +
            "WHERE CountryID = @countryID";

            putCountrySql.Parameters.AddWithValue("@name", country.Name);
            putCountrySql.Parameters.AddWithValue("@capital", country.Capital);
            putCountrySql.Parameters.AddWithValue("@countryID", id);

            // Execute query
            using (connection)
            {
                // The connection is automatically closed at the end of the using block.
                connection.Open();

                // Executes delete query and returns number of rows affected (from continents)
                int rowsAffected = putCountrySql.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}