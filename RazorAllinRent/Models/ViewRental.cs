using System;
using System.Collections.Generic;

namespace RazorAllinRent.Models;

public partial class ViewRental
{
    public int RentalId { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public int ItemTypeId { get; set; }

    public string ItemTypeName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? DueDate { get; set; }

    public int DurationDays { get; set; }

    public decimal Due { get; set; }

    public DateOnly? ReturnDate { get; set; }
}
