using EasyTrufi.Core.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Infraestructure.DTOs
{
    public class ValidatorDTO
    {
        //public long Id { get; set; }

        /// <summary>
        /// Código único o serial del hardware (ej. Dirección MAC o Serial del ESP32).
        /// </summary>
        [SwaggerSchema("Código único de identificación del dispositivo validador", Nullable = false)]
        public string ValidatorCode { get; set; } = null!;

        /// <summary>
        /// Identificador del vehículo (Placa o Número Interno) donde está instalado el validador.
        /// </summary>
        [SwaggerSchema("Placa o ID del trufi/miniván asociado", Nullable = true)]
        public string? VehicleId { get; set; }

        /// <summary>
        /// Descripción de la línea, ruta o ubicación física dentro del auto.
        /// </summary>
        [SwaggerSchema("Descripción operativa (Ej: 'Línea 210 - Trufi 45')", Nullable = false)]
        public string LocationDescription { get; set; } = null!;

        /// <summary>
        /// Estado operativo del dispositivo.
        /// </summary>
        [SwaggerSchema("Indica si el validador está habilitado para procesar cobros", Nullable = false)]
        public bool Active { get; set; }
    }
}
