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
        // Etablish a new connection using connection string in the web.config
        private static SqlConnection connection = new SqlConnection(_ConnectionString);

        /**
         * Retrieve list of all continents
         * 
         * GET api/continents
         */
        public static List<Continent> getContinents()
        {
            // Create a new list to store continents
            List<Continent> continents = new List<Continent>();

            // List to store neighbour continents
            List<NeighbourContinent> neighbourContinents;

            // SQL object that stores our SQL query

            //SELECT
            //    ContinentsA.ContinentID,
            //    ContinentsA.Name,
            //    ContinentsB.ContinentID AS NeighbourContinentID,
            //    ContinentsB.Name AS NeighbourContinentName 
            //FROM Continents ContinentsA
            //JOIN NeightbourContinents
            //    ON ContinentsA.ContinentID = NeightbourContinents.Continents_ContinentID
            //JOIN Continents ContinentsB
            //    ON ContinentsB.ContinentID = NeightbourContinents.Continents_Neighbour_ContinentID;

            SqlCommand selectContinentsSql = new SqlCommand("SELECT " +
                "ContinentsA.ContinentID," +
                "ContinentsA.Name," +
                "ContinentsB.ContinentID AS NeighbourContinentID," +
                "ContinentsB.Name AS NeighbourContinentName " +
            "FROM Continents ContinentsA " +
            "JOIN NeightbourContinents " +
                "ON ContinentsA.ContinentID = NeightbourContinents.Continents_ContinentID " +
            "JOIN Continents ContinentsB " +
                "ON ContinentsB.ContinentID = NeightbourContinents.Continents_Neighbour_ContinentID",
            connection);

            // Loop through data and push to List<Continent> continents
            using (connection)
            {
                connection.Open();

                // SQL reader
                SqlDataReader continentsList = selectContinentsSql.ExecuteReader();
                while (continentsList.Read())
                {
                    // Continent
                    Continent continent = new Continent(
                        (int)continentsList["ContinentID"],
                        (string)continentsList["Name"],
                        new List<NeighbourContinent>(),
                        null
                    );

                    // Check if continent is already in continents list
                    if (!continents.Exists(x => x.ContinentID.Equals(continent.ContinentID)))
                    {
                        // Add continent to continents list
                        continents.Add(continent);
                    }

                    NeighbourContinent neighbourContinent = new NeighbourContinent(
                        (int)continentsList["NeighbourContinentID"],
                        (string)continentsList["NeighbourContinentName"]
                    );

                    // Retrieve continent index in list
                    int continentIndex = continents.FindIndex(x => x.ContinentID.Equals(continent.ContinentID));

                    // Check if neighbour continent is already in list
                    if (!continents[continentIndex].NeighbourContinents.Exists(x => x.ContinentID.Equals(neighbourContinent.ContinentID))) {
                        // Add neighbour continent to list
                        continents[continentIndex].NeighbourContinents.Add(neighbourContinent);
                    }
                }

                return continents;
            }
        }
    }
}