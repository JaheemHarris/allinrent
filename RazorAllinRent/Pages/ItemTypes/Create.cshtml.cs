using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorAllinRent.Database;
using RazorAllinRent.Models;

namespace RazorAllinRent.Pages.ItemTypes
{
    public class CreateModel : PageModel
    {
        private readonly RazorAllinRent.Database.DatabaseContext _context;

        public CreateModel(RazorAllinRent.Database.DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ItemType ItemType { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            ItemType.IsActive = true;
            _context.ItemTypes.Add(ItemType);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
