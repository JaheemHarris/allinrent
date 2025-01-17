using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorAllinRent.Database;
using RazorAllinRent.Models;

namespace RazorAllinRent.Pages.Items
{
    public class DetailsModel : PageModel
    {
        private readonly RazorAllinRent.Database.DatabaseContext _context;

        public DetailsModel(RazorAllinRent.Database.DatabaseContext context)
        {
            _context = context;
        }

        public Item Item { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.Include(item => item.ItemType).FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                Item = item;
            }
            return Page();
        }
    }
}
