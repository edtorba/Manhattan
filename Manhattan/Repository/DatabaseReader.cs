using Manhattan.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Manhattan.Repository
{
    public class DatabaseReader
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

            // Etablish a new connection using connection string in the web.config
            SqlConnection connection = new SqlConnection(_ConnectionString);

            /**
             * SQL object that stores SQL query
             *
             * 
                SELECT
                    ContinentID, Name, NeighbourID
                FROM Continents
                JOIN NeightbourContinents
                    ON Continents.ContinentID = NeightbourContinents.Continents_ContinentID
             * 
             */
            SqlCommand selectContinentsSql = new SqlCommand("SELECT " +
                "ContinentID, Name, Continents_Neighbour_ContinentID " +
            "FROM Continents " +
            "JOIN NeightbourContinents " +
                "ON Continents.ContinentID = NeightbourContinents.Continents_ContinentID",
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
                    // Continent
                    Continent continent = new Continent(
                        (int)continentsList["ContinentID"],
                        (string)continentsList["Name"],
                        new List<int>(),
                        null
                    );

                    // Check if continent is already in continents list
                    if (!continents.Exists(x => x.ContinentID.Equals(continent.ContinentID)))
                    {
                        // Add continent to continents list
                        continents.Add(continent);
                    }

                    // Retrieve neighbour continent ID
                    int neighbourContinentID = (int)continentsList["Continents_Neighbour_ContinentID"];

                    // Retrieve continent index in list
                    int continentIndex = continents.FindIndex(x => x.ContinentID.Equals(continent.ContinentID));

                    // Check if neighbour continent is already in list
                    if (!continents[continentIndex].NeighbourContinents.Contains(neighbourContinentID))
                    {
                        // Add neighbour continent ID to list
                        continents[continentIndex].NeighbourContinents.Add(neighbourContinentID);
                    }
                }
            }

            return continents;
        }
    }
}