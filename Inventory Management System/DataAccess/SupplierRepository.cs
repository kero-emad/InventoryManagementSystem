using Inventory_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System.DataAccess
{
    public class SupplierRepository
    {
        public List<Supplier> GetAll()
        {
            List<Supplier> suppliers = new List<Supplier>();

            string query = @"
                SELECT SupplierId, SupplierName
                FROM Suppliers
                ORDER BY SupplierName";

            using (SqlConnection connection =
                   DbConnectionFactory.CreateConnection())
            {
                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader =
                           command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Supplier supplier = new Supplier
                            {
                                SupplierId = (int)reader["SupplierId"],
                                SupplierName = reader["SupplierName"].ToString()
                            };

                            suppliers.Add(supplier);
                        }
                    }
                }
            }

            return suppliers;
        }
    }
}
