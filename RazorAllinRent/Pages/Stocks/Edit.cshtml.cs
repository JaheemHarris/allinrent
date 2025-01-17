using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorAllinRent.Database;
using RazorAllinRent.Dto;
using RazorAllinRent.Models;

namespace RazorAllinRent.Pages.Stocks
{
    public class EditModel : PageModel
    {
        private readonly RazorAllinRent.Database.DatabaseContext _context;

        public EditModel(RazorAllinRent.Database.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StockUpdateDto StockUpdateDto { get; set; } = null!;

        public Item Item { get; set; } = null!;
        public StockStatusDto StockStatusDto { get; set; } = null!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Item = await _context.Items.FirstOrDefaultAsync(r => r.Id == id);
            StockStatusDto = await _context.Items
                .GroupJoin(
					_context.Stocks,
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
				}).FirstOrDefaultAsync(r => r.ItemId == id);

			if (Item == null || StockStatusDto == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
				return RedirectToPage("Edit", new { id = StockUpdateDto.ItemId });
			}

			var email = User.FindFirst(ClaimTypes.Email)?.Value;
			if (email == null)
			{
				return RedirectToPage("Edit", new { id = StockUpdateDto.ItemId });
			}

			var admin = await _context.Admins.FirstOrDefaultAsync(r => r.EmailAddress == email);

			if (admin == null)
			{
				return RedirectToPage("Edit", new { id = StockUpdateDto.ItemId });
			}


			_context.Add(new Stock 
            {
                Id = 0,
                AdminId = admin.Id,
                ItemId = StockUpdateDto.ItemId,
                Quantity = StockUpdateDto.Quantity,
            });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
