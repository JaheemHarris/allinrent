using System;
using System.Collections.Generic;

namespace RazorAllinRent.Models;

public partial class ViewItem
{
    public int Id { get; set; }

    public int ItemTypeId { get; set; }

    public string ItemTypeLabel { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImageFile { get; set; }

    public decimal RentalFee { get; set; }

    public bool IsActive { get; set; }
}
