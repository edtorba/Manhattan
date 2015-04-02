using Manhattan.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Manhattan.Repository
{
    public class Continents
    {
        private static string _ConnectionString = WebConfigurationManager
                                                    .ConnectionStrings["SOFT338_ConnectionString"]
                                                    .ConnectionString;

        /**
         * Retrieve list of all continents
         * 
         * GET api/continents
         */
        public static List<Continent> getContinents()
        {
            // Create a new list to store continents
            List<Continent> continents = new List<Continent>();

            // Establish a new connection using connection string in the web.config
            SqlConnection connection = new SqlConnection(_ConnectionString);

            /**
             * SQL object that stores SQL query
             *
             * 
                SELECT
                    ContinentID, Name, NeighbourID, Country_CountryID
                FROM Continents
                LEFT JOIN NeightbourContinents
                    ON Continents.ContinentID = NeightbourContinents.Continents_ContinentID
                LEFT JOIN Countries
	                ON Continents.ContinentID = Countries.Continents_ContinentID
             * 
             */
            SqlCommand selectContinentsSql = new SqlCommand("SELECT " +
                "ContinentID, Name, Continents_Neighbour_ContinentID, Country_CountryID " +
            "FROM Continents " +
            "LEFT JOIN NeightbourContinents " +
                "ON Continents.ContinentID = NeightbourContinents.Continents_ContinentID " +
            "LEFT JOIN Countries " +
                "ON Continents.ContinentID = Countries.Continents_ContinentID",
            connection);

            // Loop through data and push to List<Continent> continents
            using (connection)
            {
                // The connection is automatically closed at the end of the using block.
                connection.Open();

                // SQL reader
                SqlDataReader continentsList = selectContinentsSql.ExecuteReader();
                while (continentsList.Read())
                {
                    /**
                     * Continent
                     */
                    Continent continent = new Continent(
                        (int)continentsList["ContinentID"],
                        (string)continentsList["Name"],
                        new List<int>(),
                        new List<int>()
                    );

                    // Check if continent is already in continents list
                    if (!continents.Exists(x => x.ContinentID.Equals(continent.ContinentID)))
                    {
                        // Add continent to continents list
                        continents.Add(continent);
                    }

                    // Retrieve continent index in list
                    int continentIndex = continents.FindIndex(x => x.ContinentID.Equals(continent.ContinentID));

                    /**
                     * Neighbour continent
                     */
                    // Retrieve neighbour continent ID
                    if (continentsList["Continents_Neighbour_ContinentID"] != DBNull.Value)
                    {
                        int neighbourContinentID = (int)continentsList["Continents_Neighbour_ContinentID"];

                        // Check if neighbour continent is already in list
                        if (!continents[continentIndex].NeighbourContinents.Contains(neighbourContinentID))
                        {
                            // Add neighbour continent ID to list
                            continents[continentIndex].NeighbourContinents.Add(neighbourContinentID);
                        }
                    }

                    /**
                     * Countries
                     */
                    // Retrieve country ID
                    if (continentsList["Country_CountryID"] != DBNull.Value)
                    {
                        int countryID = (int)continentsList["Country_CountryID"];

                        // Check if country is already in list
                        if (!continents[continentIndex].Countries.Contains(countryID))
                        {
                            // Add country ID to list
                            continents[continentIndex].Countries.Add(countryID);
                        }
                    }
                }
            }

            return continents;
        }

        /**
         * Retrieve specific continent
         * 
         * GET api/continents/ID
         */
        public static Continent getContinent(int id)
        {
            // Initialise continent object
            Continent continent = null;

            // Establish a new connection using connection string in the web.config
            SqlConnection connection = new SqlConnection(_ConnectionString);

            /**
             * SQL object that stores SQL query
             *
             * 
                SELECT
                    ContinentID, Name, NeighbourID, Country_CountryID
                FROM Continents
                LEFT JOIN NeightbourContinents
                    ON Continents.ContinentID = NeightbourContinents.Continents_ContinentID
                LEFT JOIN Countries
	                ON Continents.ContinentID = Countries.Continents_ContinentID
                WHERE ContinentID = @id
             * 
             */
            SqlCommand selectContinentSql = new SqlCommand(null, connection);

            // Prepare statement
            selectContinentSql.CommandText = "SELECT " +
                "ContinentID, Name, Continents_Neighbour_ContinentID, Country_CountryID " +
            "FROM Continents " +
            "LEFT JOIN NeightbourContinents " +
                "ON Continents.ContinentID = NeightbourContinents.Continents_ContinentID " +
            "LEFT JOIN Countries " +
                "ON Continents.ContinentID = Countries.Continents_ContinentID " +
            "WHERE ContinentID = @id";

            SqlParameter idParam = new SqlParameter("@id", id);
            selectContinentSql.Parameters.Add(idParam);

            // Loop through data
            using (connection)
            {
                // The connection is automatically closed at the end of the using block.
                connection.Open();

                // SQL reader
                SqlDataReader continentData = selectContinentSql.ExecuteReader();

                while (continentData.Read())
                {
                    /**
                     * Continent
                     */
                    if (continent == null)
                    {
                        continent = new Continent(
                            (int)continentData["ContinentID"],
                            (string)continentData["Name"],
                            new List<int>(),
                            new List<int>()
                        );
                    }

                    /**
                     * Neighbour continent
                     */
                    // Retrieve neighbour continent ID
                    if (continentData["Continents_Neighbour_ContinentID"] != DBNull.Value)
                    {
                        int neighbourContinentID = (int)continentData["Continents_Neighbour_ContinentID"];

                        // Check if neighbour continent is already in list
                        if (!continent.NeighbourContinents.Contains(neighbourContinentID))
                        {
                            // Add neighbour continent ID to list
                            continent.NeighbourContinents.Add(neighbourContinentID);
                        }
                    }

                    /**
                     * Countries
                     */
                    // Retrieve country ID
                    if (continentData["Country_CountryID"] != DBNull.Value)
                    {
                        int countryID = (int)continentData["Country_CountryID"];

                        // Check if country is already in list
                        if (!continent.Countries.Contains(countryID))
                        {
                            // Add country ID to list
                            continent.Countries.Add(countryID);
                        }
                    }
                    
                }

            }

            return continent;
        }

        /**
         * Insert new continent
         * 
         * POST api/continents
         */
        public static Boolean postContinent(Continent continent)
        {
            // Establish a new connection using connection string in the web.config
            SqlConnection connection = new SqlConnection(_ConnectionString);

            /**
             * SQL object that stores SQL query
             *
             *
                INSERT INTO Continents
                    Name
                VALUES
                    (@name);
             * 
             */
            SqlCommand postContinentSql = new SqlCommand(null, connection);

            // Prepare statement for continent
            postContinentSql.CommandText = "INSERT INTO Continents " +
                "(Name) " +
            "output INSERTED.ContinentID VALUES " +
                "(@Name)";

            SqlParameter nameParam = new SqlParameter("@Name", continent.Name);
            postContinentSql.Parameters.Add(nameParam);

            // Execute query
            using (connection)
            {
                // The connection is automatically closed at the end of the using block.
                connection.Open();

                // Executes insert query and returns identity of the most recently added record
                int continentID = (int)postContinentSql.ExecuteScalar();

                // Insert neighbour continents
                if (continent.NeighbourContinents != null)
                {
                    foreach (var neighbour in continent.NeighbourContinents)
                    {
                        // Prepare statement for continent neighbours
                        SqlCommand continentNeighboursSql = new SqlCommand("INSERT INTO NeightbourContinents " +
                            "(Continents_ContinentID, Continents_Neighbour_ContinentID) " +
                        "VALUES " +
                            "(@ContinentID, @NeighbourID)",
                        connection);

                        continentNeighboursSql.Parameters.AddWithValue("@ContinentID", continentID);
                        continentNeighboursSql.Parameters.AddWithValue("@NeighbourID", neighbour);
                        continentNeighboursSql.ExecuteNonQuery();
                    }
                }

                // Insert continent countries
                if (continent.Countries != null)
                {
                    foreach (var country in continent.Countries)
                    {
                        // Prepare statement for continent countries
                        SqlCommand continentCountriesSql = new SqlCommand("INSERT INTO Countries " +
                            "(Country_CountryID, Continents_ContinentID) " +
                        "VALUES " +
                            "(@CountryID, @ContinentID)",
                        connection);

                        continentCountriesSql.Parameters.AddWithValue("@CountryID", country);
                        continentCountriesSql.Parameters.AddWithValue("@ContinentID", continentID);
                        continentCountriesSql.ExecuteNonQuery();
                    }
                }

                return true;
            }
        }

        /**
         * Update specific continent
         * 
         * PUT api/continents/ID
         */
        public static Boolean putContinent(int id, Continent continent)
        {
            // Establish a new connection using connection string in the web.config
            SqlConnection connection = new SqlConnection(_ConnectionString);

            /**
             * SQL object that stores SQL query
             *
             *
                UPDATE Continents
                    Name = @name
                WHERE ContinentID = @id
             * 
             */
            SqlCommand putContinentSql = new SqlCommand(null, connection);

            // Prepare statement for continent
            putContinentSql.CommandText = "UPDATE Continents " +
                "Name = @name " +
            "WHERE ContinentID = @id";

            putContinentSql.Parameters.AddWithValue("@name", continent.Name);
            putContinentSql.Parameters.AddWithValue("@id", id);

            // Execute query
            using (connection)
            {
                // The connection is automatically closed at the end of the using block.
                connection.Open();

                // Executes delete query and returns number of rows affected (from continents)
                int rowsAffected = putContinentSql.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Update neighbour continents and countries
                    SqlCommand deleteFromNeighbours = new SqlCommand(null, connection);
                    SqlCommand deleteFromCountries = new SqlCommand(null, connection);

                    deleteFromNeighbours.CommandText = "DELETE FROM NeightbourContinents WHERE Continents_ContinentID = @id";
                    deleteFromCountries.CommandText = "DELETE FROM Countries WHERE Continents_ContinentID = @id";

                    deleteFromNeighbours.Parameters.AddWithValue("@id", id);
                    deleteFromCountries.Parameters.AddWithValue("@id", id);

                    // Insert neighbour continents
                    if (continent.NeighbourContinents != null)
                    {
                        foreach (var neighbour in continent.NeighbourContinents)
                        {
                            // Prepare statement for continent neighbours
                            SqlCommand continentNeighboursSql = new SqlCommand("INSERT INTO NeightbourContinents " +
                                "(Continents_ContinentID, Continents_Neighbour_ContinentID) " +
                            "VALUES " +
                                "(@continentID, @neighbourID)",
                            connection);

                            continentNeighboursSql.Parameters.AddWithValue("@continentID", id);
                            continentNeighboursSql.Parameters.AddWithValue("@neighbourID", neighbour);
                            continentNeighboursSql.ExecuteNonQuery();
                        }
                    }

                    // Insert continent countries
                    if (continent.Countries != null)
                    {
                        foreach (var country in continent.Countries)
                        {
                            // Prepare statement for continent countries
                            SqlCommand continentCountriesSql = new SqlCommand("INSERT INTO Countries " +
                                "(Country_CountryID, Continents_ContinentID) " +
                            "VALUES " +
                                "(@countryID, @continentID)",
                            connection);

                            continentCountriesSql.Parameters.AddWithValue("@countryID", country);
                            continentCountriesSql.Parameters.AddWithValue("@continentID", id);
                            continentCountriesSql.ExecuteNonQuery();
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /**
         * Delete specific continent
         * 
         * DELETE api/continents/ID
         */
        public static Boolean deleteContinent(int id)
        {
            // Establish a new connection using connection string in the web.config
            SqlConnection connection = new SqlConnection(_ConnectionString);

            /**
             * SQL object that stores SQL query
             *
             * 
                DELETE * FROM Continents WHERE ContinentID = @id
             * 
             */
            SqlCommand deleteContinentSql = new SqlCommand(null, connection);
            SqlCommand deleteFromNeighbours = new SqlCommand(null, connection);
            SqlCommand deleteFromCountries = new SqlCommand(null, connection);

            // Prepare statement
            deleteContinentSql.CommandText = "DELETE FROM Continents WHERE ContinentID = @id";
            deleteFromNeighbours.CommandText = "DELETE FROM NeightbourContinents WHERE Continents_ContinentID = @id";
            deleteFromCountries.CommandText = "DELETE FROM Countries WHERE Continents_ContinentID = @id";

            deleteContinentSql.Parameters.AddWithValue("@id", id);
            deleteFromNeighbours.Parameters.AddWithValue("@id", id);
            deleteFromCountries.Parameters.AddWithValue("@id", id);

            // Execute query
            using (connection)
            {
                // The connection is automatically closed at the end of the using block.
                connection.Open();

                // Execute delete query (from countries)
                deleteFromCountries.ExecuteNonQuery();

                // Execute delete query (from neighbours)
                deleteFromNeighbours.ExecuteNonQuery();

                // Executes delete query and returns number of rows affected (from continents)
                int rowsAffected = deleteContinentSql.ExecuteNonQuery();

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