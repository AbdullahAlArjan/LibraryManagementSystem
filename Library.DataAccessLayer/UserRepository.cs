using System;
using System.Data;

using System.Security.Cryptography;
using System.Text;
using Library.Entities;
using Library.DataAccessLayer.Interfaces;
using Microsoft.Data.SqlClient;

namespace Library.DataAccessLayer
{
    public class UserRepository : IUserRepository
    {
        public bool ValidateLogin(string username, string password)
        {
            bool isValid = false;
            string hashedPassword = ComputeSha256Hash(password);

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT COUNT(*) 
                     FROM Users 
                     WHERE Username = @Username 
                     AND Password = @Password";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username.Trim());
            command.Parameters.AddWithValue("@Password", hashedPassword);

            try
            {
                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                isValid = count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while validating login.", ex);
            }
            finally
            {
                connection.Close();
            }

            return isValid;
        }

        public int AddUser(User user)
        {
            int userId = -1;
            string hashedPassword = ComputeSha256Hash(user.Password);

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"INSERT INTO Users (Username, Password, Role)
                     VALUES (@Username, @Password, @Role);
                     SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", user.Username.Trim());
            command.Parameters.AddWithValue("@Password", hashedPassword);
            command.Parameters.AddWithValue("@Role", user.Role);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    userId = insertedID;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the user.", ex);
            }
            finally
            {
                connection.Close();
            }

            return userId;
        }

        public bool UpdateUser(User user)
        {
            bool isUpdated = false;
            string hashedPassword = ComputeSha256Hash(user.Password);

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"UPDATE Users 
                     SET Username = @Username, 
                         Password = @Password, 
                         Role = @Role
                     WHERE UserID = @UserID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", user.Username.Trim());
            command.Parameters.AddWithValue("@Password", hashedPassword);
            command.Parameters.AddWithValue("@Role", user.Role);
            command.Parameters.AddWithValue("@UserID", user.UserID);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                isUpdated = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user.", ex);
            }
            finally
            {
                connection.Close();
            }

            return isUpdated;
        }

        public bool DeleteUser(int userId)
        {
            bool isDeleted = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = "DELETE FROM Users WHERE UserID = @UserID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", userId);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                isDeleted = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the user.", ex);
            }
            finally
            {
                connection.Close();
            }

            return isDeleted;
        }

        public DataTable GetAllUsers()
        {
            DataTable usersTable = new DataTable();

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = "SELECT UserID, Username, Role FROM Users;";
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    usersTable.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all users.", ex);
            }
            finally
            {
                connection.Close();
            }

            return usersTable;
        }

        public User GetUserById(int userId)
        {
            User user = null;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT UserID, Username, Role
                             FROM Users 
                             WHERE UserID = @UserID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", userId);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    user = new User
                    {
                        UserID = (int)reader["UserID"],
                        Username = reader["Username"].ToString(),
                        Role = reader["Role"].ToString(),
                       
                    };
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user by ID.", ex);
            }
            finally
            {
                connection.Close();
            }

            return user;
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}
