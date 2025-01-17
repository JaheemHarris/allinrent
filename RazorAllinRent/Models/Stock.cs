using System;
using System.Collections.Generic;

namespace RazorAllinRent.Models;

public partial class Stock
{
    public int Id { get; set; }

    public int AdminId { get; set; }

    public int ItemId { get; set; }

    public int Quantity { get; set; }

    public DateTime Date { get; set; }

    public virtual Admin Admin { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
