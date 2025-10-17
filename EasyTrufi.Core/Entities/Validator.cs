using System;
using System.Collections.Generic;

namespace EasyTrufi.Core.Entities;

public partial class Validator
{
    public long Id { get; set; }

    public string ValidatorCode { get; set; } = null!;

    public string? VehicleId { get; set; }

    public string LocationDescription { get; set; } = null!;

    public bool Active { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
