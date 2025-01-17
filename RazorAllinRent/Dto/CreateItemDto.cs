using RazorAllinRent.Models;
using System.ComponentModel;

namespace RazorAllinRent.Dto
{
    public class CreateItemDto
    {
        [DisplayName("Type")]
        public int ItemTypeId { get; set; }

        [DisplayName("Nom")]
        public string Name { get; set; } = null!;
        [DisplayName("Déscription")]
        public string? Description { get; set; }
        [DisplayName("Image")]
        public IFormFile ImageFile { get; set; } = null!;
        [DisplayName("Prix de location journalier")]
        public decimal RentalFee { get; set; }
    }
}
