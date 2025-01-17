using System.ComponentModel.DataAnnotations;

namespace MvcAllinRent.Models
{
    public class RentViewModel
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public DateOnly LocationDate { get; set; }
        public DateOnly DueDate { get; set; }
    }
}
