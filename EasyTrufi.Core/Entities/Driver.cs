using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace EasyTrufi.Core.Entities;

/// <summary>
/// Representa un conductor en el sistema EasyTrufi.
/// </summary>
public partial class Driver : BaseEntity
{
    //public long Id { get; set; }

    public string Cedula { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public bool Active { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
