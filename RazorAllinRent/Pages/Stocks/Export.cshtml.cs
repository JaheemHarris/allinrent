using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Geom;
using RazorAllinRent.Dto;
using iText.Kernel.Colors;

namespace RazorAllinRent.Pages.Stocks
{
    public class ExportModel : PageModel
    {
        private readonly RazorAllinRent.Database.DatabaseContext _context;

        public ExportModel(RazorAllinRent.Database.DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string? q, DateOnly? statusDate)
        {
            var dateTimeNow = DateTime.Now;
            var items = await _context.Items
               .GroupJoin(
                   _context.Stocks.Where(s => DateOnly.FromDateTime(s.Date) <= (statusDate == null ? DateOnly.FromDateTime(dateTimeNow) : statusDate)),
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
               .Where(r => String.IsNullOrWhiteSpace(q) || r.ItemName.ToLower().Contains(q.ToLower()))
               .ToListAsync();

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(memoryStream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        var headerTitle = new Paragraph("all-in-rent.")
                            .SetFontColor(new DeviceRgb(0, 151, 178))
                            .SetFontSize(24)
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        document.Add(headerTitle);

                        var subHeaderTitle = new Paragraph("Statut du stock")
                            .SetFontSize(18)
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        document.Add(subHeaderTitle);

                        var dateParagraph = new Paragraph($"Date: {statusDate ?? DateOnly.FromDateTime(dateTimeNow)}")
                            .SetFontSize(12)
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        document.Add(dateParagraph);

                        var table = new iText.Layout.Element.Table(3).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                        table.AddHeaderCell("Article");
                        table.AddHeaderCell("Quantité en stock");
                        table.AddHeaderCell("Date de dernière modification");

                        foreach (var item in items)
                        {
                            table.AddCell(item.ItemName);
                            table.AddCell(item.Quantity.ToString());
                            table.AddCell(item.LastUpdateDate.ToString());
                        }
                        document.Add(table);
                    }
                }

                
                var fileName = $"stock-statuses_{dateTimeNow.ToString("yyyyMMdd_HHmmss")}.pdf";

                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }
        }
    }
}
