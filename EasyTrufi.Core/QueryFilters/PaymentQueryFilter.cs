using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Core.QueryFilters
{
    public class PaymentQueryFilter : PaginationQueryFilter
    {
        /// <summary>
        /// Identificador de la tarjeta NFC
        /// </summary>
        [SwaggerSchema("Identificador de la tarjeta NFC", Nullable = true)]
        public long? NfcCardId { get; set; }

        /// <summary>
        /// Identificador único del usuario (pasajero) asociado a la transacción
        /// </summary>
        [SwaggerSchema("ID del usuario en el sistema", Nullable = true)]
        public long? UserId { get; set; }

        /// <summary>
        /// Monto del pasaje expresado en centavos (Ej: 200 = 2.00 Bs)
        /// </summary>
        [SwaggerSchema("Monto a cobrar en centavos", Nullable = true)]
        public long? AmountCents { get; set; }

        /// <summary>
        /// Identificador único del dispositivo validador (ESP32) instalado en el trufi
        /// </summary>
        [SwaggerSchema("ID del dispositivo validador o vehículo", Nullable = true)]
        public string? ValidatorId { get; set; }
    }
}
