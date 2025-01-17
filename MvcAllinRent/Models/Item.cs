using System.ComponentModel.DataAnnotations;

namespace MvcAllinRent.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int ItemTypeId { get; set; }
        [Display(Name = "Type d'article")]
        public string? ItemTypeName { get; set; }
        [Display(Name = "Article")]
        public string Name { get; set; } = null!;
        [Display(Name = "Déscription")]
        public string Description { get; set; } = null!;
        public string? ImageFile { get; set; }
        [Display(Name = "Prix de location journalier")]
        public decimal RentalFee { get; set; }
        public bool IsActive { get; set; }
    }
}
