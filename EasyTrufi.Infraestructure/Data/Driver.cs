using System;
using System.Collections.Generic;

namespace EasyTrufi.Infraestructure.Data;

public partial class Driver
{
    public long Id { get; set; }

    public string Cedula { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public bool Active { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
