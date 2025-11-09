using System;
using System.Collections.Generic;

namespace EasyTrufi.Core.Entities;

public partial class User : BaseEntity
{
    //public long Id { get; set; }

    public string Cedula { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string? Email { get; set; }

    public string PasswordHash { get; set; } = null!;

    public bool EmailVerified { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<NfcCard> NfcCards { get; set; } = new List<NfcCard>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Topup> Topups { get; set; } = new List<Topup>();
}
