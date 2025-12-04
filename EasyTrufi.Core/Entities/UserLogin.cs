using System;
using Swashbuckle.AspNetCore.Annotations;

namespace EasyTrufi.Core.Entities
{
    /// <summary>
    /// Representa las credenciales usadas para el inicio de sesión de un usuario.
    /// </summary>
    public class UserLogin
    {
        /// <summary>
        /// Nombre de usuario o identificador empleado para el inicio de sesión.
        /// </summary>
        [SwaggerSchema("Nombre de usuario o identificador empleado para el inicio de sesión", Nullable = false)]
        public string User { get; set; }

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        [SwaggerSchema("Contraseña del usuario", Nullable = false)]
        public string Password { get; set; }
    }
}
