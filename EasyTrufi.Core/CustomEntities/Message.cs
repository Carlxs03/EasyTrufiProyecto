using Swashbuckle.AspNetCore.Annotations;
using System;

namespace EasyTrufi.Core.CustomEntities;

/// <summary>
/// Representa un mensaje genérico utilizado en el sistema EasyTrufi.
/// </summary>
public class Message
{
    /// <summary>
    /// Tipo del mensaje (por ejemplo, "Información", "Advertencia", "Error").
    /// </summary>
    [SwaggerSchema("Tipo del mensaje (e.g., Información, Advertencia, Error)", Nullable = false)]
    public string Type { get; set; }

    /// <summary>
    /// Descripción detallada del mensaje.
    /// </summary>
    [SwaggerSchema("Descripción detallada del mensaje", Nullable = false)]
    public string Description { get; set; }
}
