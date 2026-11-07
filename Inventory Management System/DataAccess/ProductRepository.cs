using Inventory_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System.DataAccess
{
    public class ProductRepository
    {
        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();
            string query = @"
                SELECT
                    p.ProductId,
                    p.ProductName,
                    p.Description,
                    p.QuantityInStock,
                    p.MinimumStockLevel,
                    p.Price,
                    p.CategoryId,
                    c.CategoryName,
                    p.SupplierId,
                    s.SupplierName,
                    p.CreatedAt,
                    p.UpdatedAt
                FROM Products p
                INNER JOIN Categories c ON c.CategoryId = p.CategoryId
                INNER JOIN Suppliers s ON s.SupplierId = p.SupplierId
                ORDER BY p.ProductId";

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
                            Product product = new Product
                            {
                                ProductId = (int)reader["ProductId"],

                                ProductName = reader["ProductName"].ToString(),

                                Description = reader["Description"] == System.DBNull.Value
                                    ? null
                                    : reader["Description"].ToString(),

                                QuantityInStock = (int)reader["QuantityInStock"],

                                MinimumStockLevel = (int)reader["MinimumStockLevel"],

                                Price = (decimal)reader["Price"],

                                CategoryId = (int)reader["CategoryId"],

                                CategoryName = reader["CategoryName"].ToString(),

                                SupplierId = (int)reader["SupplierId"],

                                SupplierName = reader["SupplierName"].ToString(),

                                CreatedAt = (System.DateTime)reader["CreatedAt"],

                                UpdatedAt = reader["UpdatedAt"] == System.DBNull.Value
                                    ? (System.DateTime?)null
                                    : (System.DateTime)reader["UpdatedAt"]
                            };

                            products.Add(product);
                        }
                    }
                }
            }

            return products;
        }

        public int Add(Product product)
        {
            string query = @"
                INSERT INTO Products
                (
                    ProductName,
                    Description,
                    QuantityInStock,
                    MinimumStockLevel,
                    Price,
                    CategoryId,
                    SupplierId
                )
                VALUES
                (
                    @ProductName,
                    @Description,
                    @QuantityInStock,
                    @MinimumStockLevel,
                    @Price,
                    @CategoryId,
                    @SupplierId
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection =
                   DbConnectionFactory.CreateConnection())
            {
                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(
                        "@ProductName", product.ProductName);

                    command.Parameters.AddWithValue(
                        "@Description",
                        (object)product.Description ?? System.DBNull.Value);

                    command.Parameters.AddWithValue(
                        "@QuantityInStock", product.QuantityInStock);

                    command.Parameters.AddWithValue(
                        "@MinimumStockLevel", product.MinimumStockLevel);

                    command.Parameters.AddWithValue(
                        "@Price", product.Price);

                    command.Parameters.AddWithValue(
                        "@CategoryId", product.CategoryId);

                    command.Parameters.AddWithValue(
                        "@SupplierId", product.SupplierId);

                    connection.Open();

                    int newProductId =
                        (int)command.ExecuteScalar();

                    return newProductId;
                }
            }
        }
        public bool Update(Product product)
        {
            string query = @"
                UPDATE Products
                SET
                    ProductName = @ProductName,
                    Description = @Description,
                    QuantityInStock = @QuantityInStock,
                    MinimumStockLevel = @MinimumStockLevel,
                    Price = @Price,
                    CategoryId = @CategoryId,
                    SupplierId = @SupplierId,
                    UpdatedAt = SYSDATETIME()
                WHERE ProductId = @ProductId";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProductId", product.ProductId);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue(
                    "@Description",
                    (object)product.Description ?? System.DBNull.Value
                );
                command.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);
                command.Parameters.AddWithValue(
                    "@MinimumStockLevel", product.MinimumStockLevel);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                command.Parameters.AddWithValue("@SupplierId", product.SupplierId);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        public bool Delete(int productId)
        {
            string query = @"
                DELETE FROM Products
                WHERE ProductId = @ProductId";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProductId", productId);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        public List<InventoryReportItem> GetAllProductsForReport()
        {
            List<InventoryReportItem> products =
                new List<InventoryReportItem>();

            string query = @"
                SELECT
                    p.ProductId,
                    p.ProductName,
                    c.CategoryName,
                    s.SupplierName,
                    p.QuantityInStock,
                    p.MinimumStockLevel,
                    p.Price
                FROM Products p
                INNER JOIN Categories c
                    ON p.CategoryId = c.CategoryId
                INNER JOIN Suppliers s
                    ON p.SupplierId = s.SupplierId
                ORDER BY p.QuantityInStock ASC;";
            using (SqlConnection connection =
                DbConnectionFactory.CreateConnection())
            {
                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InventoryReportItem item =
                                new InventoryReportItem();

                            item.ProductId =
                                Convert.ToInt32(reader["ProductId"]);

                            item.ProductName =
                                reader["ProductName"].ToString();

                            item.CategoryName =
                                reader["CategoryName"].ToString();

                            item.SupplierName =
                                reader["SupplierName"].ToString();

                            item.QuantityInStock =
                                Convert.ToInt32(reader["QuantityInStock"]);

                            item.MinimumStockLevel =
                                Convert.ToInt32(reader["MinimumStockLevel"]);

                            item.Price =
                                Convert.ToDecimal(reader["Price"]);

                            products.Add(item);
                        }
                    }
                }
            }

            return products;
        }
    }
}
