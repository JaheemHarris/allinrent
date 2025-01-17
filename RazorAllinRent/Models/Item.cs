using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RazorAllinRent.Models;

public partial class Item
{
    public int Id { get; set; }

    public int ItemTypeId { get; set; }

    [DisplayName("Nom")]
    public string Name { get; set; } = null!;
    [DisplayName("Déscription")]
    public string? Description { get; set; }
    [DisplayName("Image")]
    public string? ImageFile { get; set; }
    [DisplayName("Prix de location journalier")]
    public decimal RentalFee { get; set; }
    [DisplayName("Actif")]
    public bool IsActive { get; set; }
    [DisplayName("Type")]
    public virtual ItemType ItemType { get; set; } = null!;

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
