using System.ComponentModel;

namespace RazorAllinRent.Dto
{
    public class StockStatusDto
    {
        public int ItemId { get; set; }
        [DisplayName("Article")]
        public string ItemName { get; set; } = string.Empty;
        [DisplayName("Quantité en stock")]
        public int Quantity { get; set; }
        [DisplayName("Date de dernière modification")]
        public DateTime? LastUpdateDate { get; set; }
    }
}
