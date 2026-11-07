using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System.DataAccess
{
    public static class DbConnectionFactory
    {
        private static readonly string _connectionString =
            ConfigurationManager
                .ConnectionStrings["InventoryDB"]
                .ConnectionString;

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
