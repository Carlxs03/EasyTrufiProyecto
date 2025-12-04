using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;
namespace EasyTrufi.Core.Entities;

/// <summary>
/// Representa el dispositivo físico (Hardware/ESP32) instalado en el vehículo para procesar cobros.
/// </summary>
public partial class Validator : BaseEntity
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

    /// <summary>
    /// Fecha de alta del dispositivo en el sistema.
    /// </summary>
    [SwaggerSchema("Fecha de registro e instalación del validador", Nullable = false)]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Fecha de la última modificación o sincronización de configuración.
    /// </summary>
    [SwaggerSchema("Fecha de última actualización de datos del dispositivo", Nullable = false)]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Historial de pagos procesados por este dispositivo específico.
    /// </summary>
    [SwaggerSchema("Colección de transacciones realizadas en este validador")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}