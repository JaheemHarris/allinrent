using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorAllinRent.Database;
using RazorAllinRent.Dto;
using RazorAllinRent.Models;
using RazorAllinRent.Utils;

namespace RazorAllinRent.Pages.Items
{
    public class CreateModel : PageModel
    {
        private readonly RazorAllinRent.Database.DatabaseContext _context;
        private readonly UploadService _uploadService;

        [BindProperty]
        public CreateItemDto CreateItemDto { get; set; } = null!;

        public CreateModel(RazorAllinRent.Database.DatabaseContext context, UploadService uploadService)
        {
            _context = context;
            _uploadService = uploadService;
        }

        public IActionResult OnGet()
        {
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "Label");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var uploadedImagePath = await _uploadService.UploadImageAsync(CreateItemDto.ImageFile);

                    _context.Items.Add(new Item {
                        Name = CreateItemDto.Name,
                        Description = CreateItemDto.Description,
                        ItemTypeId = CreateItemDto.ItemTypeId,
                        RentalFee = CreateItemDto.RentalFee,
                        ImageFile = uploadedImagePath,
                        IsActive = true 
                    });
                    await _context.SaveChangesAsync();

                    return RedirectToPage("./Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Image upload failed: {ex.Message}");
                }
                
            }
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "Label");
            return Page();
        }
    }
}
