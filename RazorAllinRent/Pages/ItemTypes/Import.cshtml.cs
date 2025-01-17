using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using RazorAllinRent.Dto;
using System.Globalization;
using RazorAllinRent.Models;
using System.Linq;
using RazorAllinRent.Mappers;

namespace RazorAllinRent.Pages.ItemTypes
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
                            csv.Context.RegisterClassMap<ItemTypeMap>();
                            var records = csv.GetRecords<ItemType>().ToList();

                            foreach (var record in records)
                            {
                                try
                                {
                                    _context.ItemTypes.Add(record);
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
