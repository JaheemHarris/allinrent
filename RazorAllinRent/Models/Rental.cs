using System;
using System.Collections.Generic;

namespace RazorAllinRent.Models;

public partial class Rental
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ItemId { get; set; }

    public int Quantity { get; set; }

    public DateOnly StartDate { get; set; }

    public int DurationDays { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Due { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public DateTime? RentalDate { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual AuthUser User { get; set; } = null!;
}
