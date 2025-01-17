using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorAllinRent.Dto;
using RazorAllinRent.Mappers;
using RazorAllinRent.Models;
using System.Globalization;

namespace RazorAllinRent.Pages.Items
{
    public class ImportModel : PageModel
    {
        private readonly RazorAllinRent.Database.DatabaseContext _context;

        public ImportModel(RazorAllinRent.Database.DatabaseContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public IFormFile? CsvFile { get; set; }
        public ImportResultDto? ImportResult { get; set; }
        public async Task<IActionResult> OnPost()
        {
            if (CsvFile == null || CsvFile.Length == 0)
            {
                ModelState.AddModelError("CsvFile", "Veuillez importer un fichier CSV");
                return Page();
            }

            var allowedExtensions = new[] { ".csv" };
            var fileExtension = Path.GetExtension(CsvFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("CsvFile", "Seul les fichiers CSV sont autorisés.");
                return Page();
            }

            ImportResult = new ImportResultDto();

            using (var stream = CsvFile.OpenReadStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    using (var csv = new CsvHelper.CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        PrepareHeaderForMatch = args => args.Header.ToLower(),
                    }))
                    {
                        try
                        {
                            csv.Read();
                            csv.ReadHeader();
                            var records = csv.GetRecords<ItemCsvDto>().ToList();

                            foreach (var record in records)
                            {
                                try
                                {
                                    var itemType = _context.ItemTypes.FirstOrDefault(x => x.Label == record.ItemType);
                                    if (itemType == null)
                                    {
                                        throw new Exception("Le type d'article n'existe pas");
                                    }

                                    var newItem = new Item
                                    {
                                        ItemTypeId = itemType.Id,
                                        Name = record.Name,
                                        Description = record.Description?.Trim(),
                                        RentalFee = record.RentalFee,
                                        IsActive = true
                                    };

                                    _context.Items.Add(newItem);
                                    ImportResult.SuccessCount++;
                                }
                                catch (Exception)
                                {
                                    ImportResult.FailureCount++;
                                }
                            }

                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("CsvFile", "Echec du traitement du fichier CSV. Erreur: " + ex.Message);
                            return Page();
                        }
                    }

                    return Page();
                }
            }
        }
    }
}
