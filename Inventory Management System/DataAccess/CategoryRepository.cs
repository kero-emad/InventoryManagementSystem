using Inventory_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System.DataAccess
{
    public class CategoryRepository
    {
        public List<Category> GetAll()
        {
            List<Category> categories = new List<Category>();

            string query = @"
                SELECT CategoryId, CategoryName
                FROM Categories
                ORDER BY CategoryName";

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
                            Category category = new Category
                            {
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = reader["CategoryName"].ToString()
                            };

                            categories.Add(category);
                        }
                    }
                }
            }

            return categories;
        }
    }
}
