using EasyTrufi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Infraestructure.DTOs
{
    public class PaymentDTO
    {
        public long NfcCardId { get; set; }

        public long? UserId { get; set; }

        public long AmountCents { get; set; }

        public string? ValidatorId { get; set; }

        //public DateTime CreatedAt { get; set; }

        //public virtual NfcCard NfcCard { get; set; } = null!;

        //public virtual User? User { get; set; }

        //public virtual Validator? Validator { get; set; }



    }
}
