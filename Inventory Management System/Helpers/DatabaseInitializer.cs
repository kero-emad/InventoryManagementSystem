using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System.Helpers
{
    public static class DatabaseInitializer
    {
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection connection =
                    DataAccess.DbConnectionFactory.CreateConnection())
                {
                    connection.Open();

                    return connection.State ==
                           System.Data.ConnectionState.Open;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(
                    "Database connection failed: " + ex.Message);
            }
        }
    }
}
