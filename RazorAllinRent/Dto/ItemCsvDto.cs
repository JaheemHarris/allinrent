namespace RazorAllinRent.Dto
{
    public class ItemCsvDto
    {
        public string ItemType { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public decimal RentalFee { get; set; }
    }
}
