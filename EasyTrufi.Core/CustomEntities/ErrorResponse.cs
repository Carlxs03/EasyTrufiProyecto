using System;
using EasyTrufi.Core.CustomEntities;
using Swashbuckle.AspNetCore.Annotations;

namespace EasyTrufi.Core.CustomEntities
{
    /// <summary>
    /// Representa una respuesta de error estándar utilizada en el sistema EasyTrufi.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Tipo del error (por ejemplo, "Validación", "Autenticación", etc.).
        /// </summary>
        [SwaggerSchema("Tipo del error (e.g., Validación, Autenticación)", Nullable = false)]
        public string Type { get; set; }

        /// <summary>
        /// Mensaje descriptivo del error.
        /// </summary>
        [SwaggerSchema("Mensaje descriptivo del error", Nullable = false)]
        public string Message { get; set; }

        /// <summary>
        /// Código único que identifica el error.
        /// </summary>
        [SwaggerSchema("Código único del error", Nullable = false)]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Marca de tiempo que indica cuándo ocurrió el error.
        /// </summary>
        [SwaggerSchema("Fecha y hora en formato UTC cuando ocurrió el error", Nullable = false)]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Ruta o endpoint donde ocurrió el error.
        /// </summary>
        [SwaggerSchema("Ruta o endpoint donde ocurrió el error", Nullable = false)]
        public string Path { get; set; }

        /// <summary>
        /// Constructor que inicializa la marca de tiempo con la fecha y hora actual en UTC.
        /// </summary>
        public ErrorResponse()
        {
            Timestamp = DateTime.UtcNow;
        }

    }
}
