using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace EasyTrufi.Core.Entities;

/// <summary>
/// Representa a un usuario (pasajero) registrado en el sistema EasyTrufi.
/// </summary>
public partial class User : BaseEntity
{
    //public long Id { get; set; }

    /// <summary>
    /// Cédula de identidad del usuario.
    /// </summary>
    [SwaggerSchema("Número de Cédula de Identidad (C.I.) del usuario", Nullable = false)]
    public string Cedula { get; set; } = null!;

    /// <summary>
    /// Nombre completo del usuario.
    /// </summary>
    [SwaggerSchema("Nombre completo del usuario (Nombres y Apellidos)", Nullable = false)]
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Fecha de nacimiento del usuario.
    /// </summary>
    [SwaggerSchema("Fecha de nacimiento (usado para validar edad o descuentos)", Nullable = false)]
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Correo electrónico de contacto.
    /// </summary>
    [SwaggerSchema("Dirección de correo electrónico del usuario", Nullable = true)]
    public string? Email { get; set; }

    /// <summary>
    /// Hash de seguridad de la contraseña.
    /// </summary>
    [SwaggerSchema("Hash encriptado de la contraseña del usuario", Nullable = false)]
    public string PasswordHash { get; set; } = null!;

    /// <summary>
    /// Indica si el correo electrónico ha sido verificado.
    /// </summary>
    [SwaggerSchema("Indica si el usuario ha verificado su correo electrónico", Nullable = false)]
    public bool EmailVerified { get; set; }

    /// <summary>
    /// Fecha y hora de creación del registro.
    /// </summary>
    [SwaggerSchema("Fecha de registro del usuario en el sistema", Nullable = false)]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Fecha y hora de la última actualización del registro.
    /// </summary>
    [SwaggerSchema("Fecha de la última modificación de los datos del usuario", Nullable = false)]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Colección de tarjetas NFC asociadas al usuario.
    /// </summary>
    [SwaggerSchema("Lista de tarjetas NFC vinculadas a este usuario")]
    public virtual ICollection<NfcCard> NfcCards { get; set; } = new List<NfcCard>();

    /// <summary>
    /// Historial de pagos realizados por el usuario.
    /// </summary>
    [SwaggerSchema("Historial de pagos de pasajes realizados")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    /// <summary>
    /// Historial de recargas de saldo realizadas por el usuario.
    /// </summary>
    [SwaggerSchema("Historial de recargas de crédito en la cuenta")]
    public virtual ICollection<Topup> Topups { get; set; } = new List<Topup>();
}