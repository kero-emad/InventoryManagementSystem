using Inventory_Management_System.Helpers;
using Inventory_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System.DataAccess
{
    public class UserRepository
    {
        public bool CreateUser(string username, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username and password are required.");
            }

            if (role != "Admin" && role != "Viewer")
            {
                throw new ArgumentException("Invalid user role.");
            }

            byte[] passwordHash;
            byte[] passwordSalt;

            PasswordHasher.HashPassword(
                password,
                out passwordHash,
                out passwordSalt);

            using (SqlConnection connection =
                DbConnectionFactory.CreateConnection())
            {
                string query = @"
                    INSERT INTO Users
                    (
                        Username,
                        PasswordHash,
                        PasswordSalt,
                        Role
                    )
                    VALUES
                    (
                        @Username,
                        @PasswordHash,
                        @PasswordSalt,
                        @Role
                    )";

                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    command.Parameters.Add(
                        "@Username", SqlDbType.NVarChar, 50).Value = username.Trim();

                    command.Parameters.Add(
                        "@PasswordHash", SqlDbType.VarBinary, 64).Value = passwordHash;

                    command.Parameters.Add(
                        "@PasswordSalt", SqlDbType.VarBinary, 32).Value = passwordSalt;

                    command.Parameters.Add(
                        "@Role", SqlDbType.NVarChar, 20).Value = role;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrEmpty(password))
            {
                return null;
            }

            User user = GetByUsername(username.Trim());

            if (user == null)
            {
                return null;
            }

            bool isPasswordValid = PasswordHasher.VerifyPassword(
                password,
                user.PasswordHash,
                user.PasswordSalt);

            if (!isPasswordValid)
            {
                return null;
            }
            user.PasswordHash = null;
            user.PasswordSalt = null;

            return user;
        }
        private User GetByUsername(string username)
        {
            using (SqlConnection connection =
                DbConnectionFactory.CreateConnection())
            {
                string query = @"
                    SELECT
                        UserId,
                        Username,
                        PasswordHash,
                        PasswordSalt,
                        Role,
                        CreatedAt
                    FROM Users
                    WHERE Username = @Username";

                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    command.Parameters.Add(
                        "@Username", SqlDbType.NVarChar, 50).Value = username;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return null;
                        }
                        User user = new User
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            Username = reader.GetString(reader.GetOrdinal("Username")),
                            PasswordHash = (byte[])reader["PasswordHash"],
                            PasswordSalt = (byte[])reader["PasswordSalt"],
                            Role = reader.GetString(reader.GetOrdinal("Role")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                        };
                        return user;
                    }
                }
            }
        }
        public bool HasAdmin()
        {
            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Role = @Role";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Role", SqlDbType.NVarChar, 20).Value = "Admin";

                    connection.Open();

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }
    }
}
