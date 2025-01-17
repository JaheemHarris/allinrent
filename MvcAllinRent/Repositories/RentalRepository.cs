using Microsoft.Data.SqlClient;
using MvcAllinRent.Models;

namespace MvcAllinRent.Repositories
{
    public class RentalRepository
    {
        public readonly string _connectionString = null!;

        public RentalRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<PaginatedResult<Rental>> GetUserRentals(int userId, int? pageNumber = 1, int? pageSize = 1, string? searchItemName = null)
        {
            var rentals = new List<Rental>();
            int totalCount = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        COUNT(*) OVER() AS TotalCount,
                        [RentalId], [UserId], [FirstName], [LastName], 
                        [ItemId], [ItemName], [ItemTypeId], [ItemTypeName], 
                        [Quantity], [UnitPrice], [StartDate], [DurationDays], 
                        [Due], [DueDate], [ReturnDate]
                    FROM [View_Rental]
                    WHERE UserId = @UserId
                    AND (@SearchItemName IS NULL OR ItemName LIKE '%' + @SearchItemName + '%')
                    ORDER BY RentalId
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@SearchItemName", (object?)searchItemName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize ?? 0);
                    command.Parameters.AddWithValue("@PageSize", pageSize ?? 10);

                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        if (rentals.Count == 0)
                        {
                            totalCount = Convert.ToInt32(reader["TotalCount"]); ;
                        }

                        var rental = new Rental
                        {
                            Id = Convert.ToInt32(reader["RentalId"]),
                            UserId = Convert.ToInt32(reader["UserId"]),
                            UserFirstName = reader["FirstName"].ToString()!,
                            UserLastName = reader["LastName"].ToString()!,
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            ItemName = reader["ItemName"].ToString()!,
                            ItemTypeId = Convert.ToInt32(reader["ItemTypeId"]),
                            ItemTypeName = reader["ItemTypeName"].ToString()!,
                            Due = Convert.ToDecimal(reader["Due"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            DurationDays = Convert.ToInt32(reader["DurationDays"]),
                            DueDate = reader["DueDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["DueDate"]),
                            ReturnDate = reader["ReturnDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["ReturnDate"])
                        };

                        rentals.Add(rental);
                    }
                }
            }

            return new PaginatedResult<Rental> {
                Items = rentals,
                PageIndex = pageNumber.GetValueOrDefault(1),
                PageSize = pageSize.GetValueOrDefault(2),
                SearchCriteria = searchItemName,
                TotalCount = totalCount,
            };
        }

        public async Task<bool> Save(Rental rental)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO Rental (UserId, ItemId, Quantity, UnitPrice, StartDate, DurationDays, Due)
                    VALUES (@UserId, @ItemId, @Quantity, @UnitPrice, @StartDate, @DurationDays, @Due);
                ";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", rental.UserId);
                    command.Parameters.AddWithValue("@ItemId", rental.ItemId);
                    command.Parameters.AddWithValue("@Quantity", rental.Quantity);
                    command.Parameters.AddWithValue("@UnitPrice", rental.UnitPrice);
                    command.Parameters.AddWithValue("@StartDate", rental.StartDate);
                    command.Parameters.AddWithValue("@DurationDays", rental.DurationDays);
                    command.Parameters.AddWithValue("@Due", rental.Due);

                    await connection.OpenAsync();
                    int affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows > 0;
                }
            }
        }

        public async Task<bool> Update(Rental rental)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE Rental
                    SET 
                        UserId = @UserId,
                        ItemId = @ItemId,
                        Quantity = @Quantity,
                        UnitPrice = @UnitPrice,
                        StartDate = @StartDate,
                        DurationDays = @DurationDays,
                        Due = @Due,
                        ReturnDate = @ReturnDate
                    WHERE Id = @RentalId;
                ";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", rental.UserId);
                    command.Parameters.AddWithValue("@ItemId", rental.ItemId);
                    command.Parameters.AddWithValue("@Quantity", rental.Quantity);
                    command.Parameters.AddWithValue("@UnitPrice", rental.UnitPrice);
                    command.Parameters.AddWithValue("@StartDate", rental.StartDate);
                    command.Parameters.AddWithValue("@DurationDays", rental.DurationDays);
                    command.Parameters.AddWithValue("@Due", rental.Due);
                    command.Parameters.AddWithValue("@ReturnDate", rental.ReturnDate.HasValue ? rental.ReturnDate.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@RentalId", rental.Id);

                    await connection.OpenAsync();
                    int affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows > 0;
                }
            }
        }
    }
}
