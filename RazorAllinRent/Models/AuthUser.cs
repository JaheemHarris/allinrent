using System;
using System.Collections.Generic;

namespace RazorAllinRent.Models;

public partial class AuthUser
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string IdNumber { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte Status { get; set; }

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
