using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace EasyTrufi.Core.Entities;

/// <summary>
/// Representa una transacción de recarga de saldo (crédito) a una tarjeta NFC.
/// </summary>
public partial class Topup : BaseEntity
{
    //public long Id { get; set; }

    public long NfcCardId { get; set; }

    public long? UserId { get; set; }

    public long AmountCents { get; set; }

    public string Method { get; set; } = null!;

    public string? Reference { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual NfcCard NfcCard { get; set; } = null!;

    public virtual User? User { get; set; }
}