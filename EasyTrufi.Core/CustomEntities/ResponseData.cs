using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Text.Json.Serialization;

namespace EasyTrufi.Core.CustomEntities;

/// <summary>
/// Representa una respuesta estándar utilizada en el sistema EasyTrufi, que incluye datos de paginación, mensajes y un código de estado HTTP.
/// </summary>
public class ResponseData
{
    /// <summary>
    /// Información de paginación asociada a la respuesta.
    /// </summary>
    [SwaggerSchema("Información de paginación asociada a la respuesta", Nullable = true)]
    public PagedList<object> Pagination { get; set; }

    /// <summary>
    /// Lista de mensajes relacionados con la respuesta.
    /// </summary>
    [SwaggerSchema("Lista de mensajes relacionados con la respuesta", Nullable = false)]
    public Message[] Messages { get; set; }

    /// <summary>
    /// Código de estado HTTP asociado a la respuesta.
    /// </summary>
    [JsonIgnore]
    [SwaggerSchema("Código de estado HTTP asociado a la respuesta", Nullable = false)]
    public HttpStatusCode StatusCode { get; set; }
}
