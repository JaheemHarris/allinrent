using System;
using System.Collections.Generic;

namespace RazorAllinRent.Models;

public partial class Admin
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
