using Microsoft.Data.SqlClient;
using MvcAllinRent.Models;

namespace MvcAllinRent.Repositories
{
    public class AuthUserRepository
    {
        public readonly string _connectionString = null!;

        public AuthUserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<AuthUser?> GetUserByEmail(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT [Id], [FirstName], [LastName], [EmailAddress], [PhoneNumber], [IdNumber], [Password] FROM AuthUser WHERE [EmailAddress] = @EmailAddress";

                using (var command = new SqlCommand(query, connection))
                {
                    // Add parameter to prevent SQL injection.
                    command.Parameters.AddWithValue("@EmailAddress", email);

                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        return new AuthUser
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FirstName = reader["FirstName"].ToString()!,
                            LastName = reader["LastName"].ToString()!,
                            EmailAddress = reader["EmailAddress"].ToString()!,
                            PhoneNumber = reader["PhoneNumber"].ToString()!,
                            IdNumber = reader["IdNumber"].ToString()!,
                            Password = reader["Password"].ToString()!,
                        };
                    }
                }
            }
            return null;
        }

        public async Task<bool> Save(AuthUser authUser)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Hash the password before saving
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(authUser.Password);

                string query = @"
                    INSERT INTO AuthUser ([FirstName], [LastName], [EmailAddress], [PhoneNumber], [IdNumber], [Password], [Status])
                    VALUES (@FirstName, @LastName, @EmailAddress, @PhoneNumber, @IdNumber, @Password, @Status);
                ";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", authUser.FirstName);
                    command.Parameters.AddWithValue("@LastName", authUser.LastName);
                    command.Parameters.AddWithValue("@EmailAddress", authUser.EmailAddress);
                    command.Parameters.AddWithValue("@PhoneNumber", authUser.PhoneNumber);
                    command.Parameters.AddWithValue("@IdNumber", authUser.IdNumber);
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.Parameters.AddWithValue("@Status", authUser.Status);

                    await connection.OpenAsync();
                    int affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows > 0;
                }
            }
        }

        public bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashedPassword);
        }
    }
}
