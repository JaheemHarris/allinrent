using System.ComponentModel.DataAnnotations;

namespace MvcAllinRent.Models
{
    public class Rental
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }
        public int? ItemTypeId { get; set; }
        [Display(Name = "Type")]
        public string? ItemTypeName { get; set; }
        public int ItemId { get; set; }
        [Display(Name = "Article")]
        public string? ItemName { get; set; }
        [Display(Name = "Quantité")]
        public int Quantity { get; set; }
        [Display(Name = "Prix Unitaire")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "Date de début de location")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Duration in days. Must be greater than 0.
        /// </summary>
        [Display(Name = "Durée (en jours)")]
        public int DurationDays { get; set; }

        /// <summary>
        /// The amount due. Must be greater than or equal to 0.
        /// </summary>
        [Display(Name = "Montant due")]
        public decimal Due { get; set; }

        /// <summary>
        /// The date when the rented item should be returned.
        /// </summary>
        [Display(Name = "À rendre le")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// The date when the rented item was returned.
        /// </summary>
        public DateTime? ReturnDate { get; set; }
        public string? Status;
    }

}
