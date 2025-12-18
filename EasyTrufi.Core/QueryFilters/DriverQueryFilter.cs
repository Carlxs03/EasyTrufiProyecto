using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Core.QueryFilters
{
    public class DriverQueryFilter : PaginationQueryFilter
    {
        public string Cedula { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public bool Active { get; set; }

    }
}
