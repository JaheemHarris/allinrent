using Microsoft.Data.SqlClient;
using MvcAllinRent.Models;
using System;

namespace MvcAllinRent.Repositories
{
    public class ItemTypeRepository
    {
        public readonly string _connectionString = null!;

        public ItemTypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IList<ItemType>> GetAll()
        {
            var itemTypes = new List<ItemType>();
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT [Id], [Label] ,[IsActive] FROM [ItemType] WHERE [IsActive] = 1";
                using (var command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        var item = new ItemType
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Label = reader["Label"].ToString()!,
                            IsActive = Convert.ToBoolean(reader["IsActive"])
                        };
                        itemTypes.Add(item);
                    }
                }
            }
            return itemTypes;
        }
    }
}
