using Microsoft.Data.SqlClient;
using MvcAllinRent.Models;

namespace MvcAllinRent.Repositories
{
    public class ItemRepository
    {
        public readonly string _connectionString = null!;

        public ItemRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<PaginatedResult<Item>> GetAll(int? pageNumber = 1, int? pageSize = 10, string? searchCriteria = null)
        {
            var items = new List<Item>();
            int totalCount = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        COUNT(*) OVER() AS TotalCount,
                        [Id],
                        [ItemTypeId], [ItemTypeLabel], [Name], 
                        [Description], [ImageFile], [RentalFee], [IsActive]
                    FROM [View_Item]
                    WHERE IsActive = 1 AND (@SearchCriteria IS NULL OR Name LIKE '%' + @SearchCriteria + '%')
                    ORDER BY Id
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                using (var command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@SearchCriteria", (object?)searchCriteria ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize.GetValueOrDefault(0));
                    command.Parameters.AddWithValue("@PageSize", pageSize.GetValueOrDefault(10));

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        if (items.Count == 0)
                        {
                            totalCount = Convert.ToInt32(reader["TotalCount"]); ;
                        }

                        var item = new Item
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            ItemTypeId = Convert.ToInt32(reader["ItemTypeId"]),
                            ItemTypeName = reader["ItemTypeLabel"].ToString()!,
                            Name = reader["Name"].ToString()!,
                            Description = reader["Description"].ToString()!,
                            ImageFile = reader["ImageFile"] == DBNull.Value ? null : reader["ImageFile"].ToString(),
                            RentalFee = Convert.ToDecimal(reader["RentalFee"])!,
                            IsActive = Convert.ToBoolean(reader["IsActive"])
                        };
                        items.Add(item);
                    }
                }
            }

            return new PaginatedResult<Item>
            {
                Items = items,
                PageIndex = pageNumber.GetValueOrDefault(1),
                PageSize = pageSize.GetValueOrDefault(10),
                SearchCriteria = searchCriteria,
                TotalCount = totalCount,
            };
        }

        public async Task<Item?> GetById(int itemId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT [Id], [ItemTypeId] ,[ItemTypeLabel], [Name], [Description], [ImageFile], [RentalFee], [IsActive] FROM [View_Item] WHERE [Id] = @ItemId";

                using (var command = new SqlCommand(query, connection))
                {
                    // Add parameter to prevent SQL injection.
                    command.Parameters.AddWithValue("@ItemId", itemId);

                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        return new Item
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            ItemTypeId = Convert.ToInt32(reader["ItemTypeId"]),
                            ItemTypeName = reader["ItemTypeLabel"].ToString()!,
                            Name = reader["Name"].ToString()!,
                            Description = reader["Name"].ToString()!,
                            ImageFile = reader["ImageFile"] == DBNull.Value ? null : reader["ImageFile"].ToString(),
                            RentalFee = Convert.ToDecimal(reader["RentalFee"])!,
                            IsActive = Convert.ToBoolean(reader["IsActive"])
                        };
                    }
                }
            }
            return null;
        }
    }
}
