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

namespace RazorAllinRent.Pages.Items
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly int PageSize = 10;

        public IndexModel(DatabaseContext context)
        {
            _context = context;
        }

        public PaginatedResultDto<Item> PaginatedResult { get;set; } = default!;
        public IList<ItemType> ItemTypes { get; set; } = new List<ItemType>();
        public int? ItemTypeId { get; set; }
        public async Task<IActionResult> OnGetAsync(int? pageNumber = 1, int? itemType = null, string? q = null)
        {
            var filteredQuery = _context.Items
                .Include(i => i.ItemType)
                .Where(r => itemType == null || r.ItemTypeId == itemType)
                .Where(r => String.IsNullOrWhiteSpace(q) || r.Name.ToLower().Contains(q.ToLower()));

            var totalResults = await filteredQuery.CountAsync();

            var items = await filteredQuery
                .Skip((pageNumber.GetValueOrDefault(1) - 1) * PageSize)
		        .Take(PageSize)
		        .ToListAsync();

            ItemTypes = await _context.ItemTypes.ToListAsync();

			PaginatedResult = new PaginatedResultDto<Item>
            {
                PageIndex = pageNumber.GetValueOrDefault(1),
                PageSize = PageSize,
                SearchCriteria = q,
                TotalCount = totalResults,
                Items = items,
            };

            ItemTypeId = itemType;

            return Page();
        }
    }
}
