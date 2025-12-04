using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Enum;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace EasyTrufi.Infraestructure.Data;

/// <summary>
/// Representa la información de seguridad de un usuario en el sistema EasyTrufi.
/// </summary>
public partial class Security : BaseEntity
{

    /// <summary>
    /// Contraseña asociada al usuario.
    /// </summary>
    [SwaggerSchema("Contraseña asociada al usuario", Nullable = false)]
    public string Password { get; set; } = null!;

    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    [SwaggerSchema("Nombre del usuario", Nullable = false)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Rol asignado al usuario (por ejemplo, "Administrador", "Usuario", etc.).
    /// </summary>
    [SwaggerSchema("Rol asignado al usuario (e.g., Administrador, Usuario)", Nullable = false)]
    public RoleType? Role { get; set; }
    public string Login { get; set; }
}
