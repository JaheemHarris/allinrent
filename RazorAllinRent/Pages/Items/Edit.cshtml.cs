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
    public class EditModel : PageModel
    {
        private readonly RazorAllinRent.Database.DatabaseContext _context;
        private readonly UploadService  _uploadService;

        public EditModel(RazorAllinRent.Database.DatabaseContext context, UploadService uploadService)
        {
            _context = context;
            _uploadService = uploadService;
        }

        [BindProperty]
        public UpdateItemDto UpdateItem { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item =  await _context.Items.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            UpdateItem = new UpdateItemDto
            {
                Id = item.Id,
                ItemTypeId = item.ItemTypeId,
                Name = item.Name,
                Description = item.Description,
                RentalFee = item.RentalFee,
                ImagePath = item.ImageFile,
                IsActive = item.IsActive
            };
            ViewData["ItemTypeLabel"] = new SelectList(_context.ItemTypes, "Id", "Label");
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var uploadedImagePath = UpdateItem.ImagePath;
                if (UpdateItem.ImageFile != null)
                {
                    uploadedImagePath = await _uploadService.UploadImageAsync(UpdateItem.ImageFile);
                }
                if(String.IsNullOrEmpty(uploadedImagePath))
                {
                    ModelState.AddModelError("UpdateItem.ImageFile", "Veuillez uploadez une image");
                    ViewData["ItemTypeLabel"] = new SelectList(_context.ItemTypes, "Id", "Label");
                    return Page();
                }

                _context.Attach(new Item
                {
                    Id = UpdateItem.Id,
                    ItemTypeId = UpdateItem.ItemTypeId,
                    Name = UpdateItem.Name,
                    Description = UpdateItem.Description,
                    RentalFee = UpdateItem.RentalFee,
                    ImageFile = uploadedImagePath,
                    IsActive = UpdateItem.IsActive
                }).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(UpdateItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToPage("./Index");
            }

            ViewData["ItemTypeLabel"] = new SelectList(_context.ItemTypes, "Id", "Label");
            return Page();
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
