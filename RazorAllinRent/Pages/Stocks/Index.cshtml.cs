using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorAllinRent.Database;
using RazorAllinRent.Dto;
using RazorAllinRent.Models;

namespace RazorAllinRent.Pages.Stocks
{
    public class IndexModel : PageModel
    {
        private readonly RazorAllinRent.Database.DatabaseContext _context;
        private readonly int PageSize = 10;

        public IndexModel(RazorAllinRent.Database.DatabaseContext context)
        {
            _context = context;
        }

        public PaginatedResultDto<StockStatusDto> PaginatedResult { get; set; } = default!;
        public DateOnly? StatusDate = null;
        public async Task<IActionResult> OnGetAsync(int? pageNumber = 1, string? q = null, DateOnly? statusDate = null)
        {
            StatusDate = statusDate == null ? DateOnly.FromDateTime(DateTime.Now) : statusDate;

            var filteredQuery = _context.Items
               .GroupJoin(
                   _context.Stocks.Where(s => DateOnly.FromDateTime(s.Date) <= StatusDate ),
                   item => item.Id,
                   stock => stock.ItemId,
                   (item, stocks) => new
                   {
                       ItemId = item.Id,
                       ItemName = item.Name,
                       Quantity = stocks.Sum(s => s.Quantity),
                       LastUpdateDate = stocks.OrderByDescending(s => s.Date).FirstOrDefault().Date
                   })
               .Select(result => new StockStatusDto
               {
                   ItemId = result.ItemId,
                   ItemName = result.ItemName,
                   Quantity = result.Quantity,
                   LastUpdateDate = result.LastUpdateDate
               })
               .Where(r => String.IsNullOrWhiteSpace(q) || r.ItemName.ToLower().Contains(q.ToLower()));

            var totalResults = await filteredQuery.CountAsync();

            var stockStatuses = await filteredQuery
                .Skip((pageNumber.GetValueOrDefault(1) - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            PaginatedResult = new PaginatedResultDto<StockStatusDto>
            {
                PageIndex = pageNumber.GetValueOrDefault(1),
                PageSize = PageSize,
                SearchCriteria = q,
                TotalCount = totalResults,
                Items = stockStatuses
            };

            return Page();
        }
    }
}
