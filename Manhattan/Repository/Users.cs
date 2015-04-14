using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Manhattan.Repository
{
    public class Users
    {
        private static string _ConnectionString = WebConfigurationManager
                                                    .ConnectionStrings["SOFT338_ConnectionString"]
                                                    .ConnectionString;

        /**
         * Check if valid user credentials were submitted.
         */
        public static Boolean verifyUser(string username, string password)
        {
            // Establish a new connection using connection string in the web.config
            SqlConnection connection = new SqlConnection(_ConnectionString);

            /**
             * SQL query
             *
                SELECT
                    Username, Password
                FROM Users
                WHERE Username = @username AND Password = @password
             * 
             */
            SqlCommand selectUserSql = new SqlCommand(null, connection);

            // Prepare statement
            selectUserSql.CommandText = "SELECT " +
                "Username, Password " +
            "FROM Users " +
            "WHERE Username = @username";

            selectUserSql.Parameters.AddWithValue("@username", username);

            // Loop through data
            using (connection)
            {
                // The connection is automatically closed at the end of the using block.
                connection.Open();

                // SQL reader
                SqlDataReader userData = selectUserSql.ExecuteReader();

                Boolean validUser = false;
                while(userData.Read())
                {
                    if (userData["Password"] != DBNull.Value)
                    {
                        if (BCrypt.Net.BCrypt.Verify(password, (string)userData["Password"]))
                        {
                            validUser = true;
                        }
                    }
                }

                return validUser;
            }
        }
    }
}