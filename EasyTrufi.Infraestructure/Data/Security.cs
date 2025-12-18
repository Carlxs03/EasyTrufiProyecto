using System;
using System.Collections.Generic;

namespace EasyTrufi.Infraestructure.Data;

public partial class Security
{
    public long Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Role { get; set; } = null!;
}
