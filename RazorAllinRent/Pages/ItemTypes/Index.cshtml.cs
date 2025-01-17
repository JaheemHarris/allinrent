using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorAllinRent.Database;
using RazorAllinRent.Models;

namespace RazorAllinRent.Pages.ItemTypes
{
    public class IndexModel : PageModel
    {
        private readonly RazorAllinRent.Database.DatabaseContext _context;

        public IndexModel(RazorAllinRent.Database.DatabaseContext context)
        {
            _context = context;
        }

        public IList<ItemType> ItemType { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ItemType = await _context.ItemTypes.ToListAsync();
        }
    }
}
