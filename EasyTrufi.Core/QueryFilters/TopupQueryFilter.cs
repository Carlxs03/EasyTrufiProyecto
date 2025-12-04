using EasyTrufi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Core.QueryFilters
{
    public class TopupQueryFilter : PaginationQueryFilter
    {
        public long NfcCardId { get; set; }

        public long? UserId { get; set; }

        public long AmountCents { get; set; }

        public string Method { get; set; } = null!;

        public string? Reference { get; set; }

        public string Status { get; set; } = null!;

    }
}
