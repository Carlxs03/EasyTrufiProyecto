using System;
using System.Collections.Generic;

namespace EasyTrufi.Infraestructure.Data;

public partial class Payment
{
    public long Id { get; set; }

    public long NfcCardId { get; set; }

    public long? UserId { get; set; }

    public long AmountCents { get; set; }

    public string? ValidatorId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual NfcCard NfcCard { get; set; } = null!;

    public virtual User? User { get; set; }

    public virtual Validator? Validator { get; set; }
}
