using EasyTrufi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Infraestructure.DTOs
{
    public class NfcCardBalanceDTO
    {
        public long Id { get; set; }

        public string Uid { get; set; } = null!;

        public long? UserId { get; set; }

        public bool Active { get; set; }

        //public DateTime IssuedAt { get; set; }

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public virtual ICollection<Topup> Topups { get; set; } = new List<Topup>();

        //public virtual User? User { get; set; }
    }
}
