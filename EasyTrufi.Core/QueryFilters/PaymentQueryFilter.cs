using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Core.QueryFilters
{
    public class PaymentQueryFilter : PaginationQueryFilter
    {
        public long? NfcCardId { get; set; }

        public long? UserId { get; set; }

        public long? AmountCents { get; set; }

        public string? ValidatorId { get; set; }
    }
}
