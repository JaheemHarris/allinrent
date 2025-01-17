using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RazorAllinRent.Models;

public partial class ItemType
{
    public int Id { get; set; }

    [DisplayName("Libellé")]
    public string Label { get; set; } = null!;
    [DisplayName("Actif")]
    public bool IsActive { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
