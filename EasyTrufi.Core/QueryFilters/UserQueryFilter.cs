using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace EasyTrufi.Core.QueryFilters
{
    public class UserQueryFilter : PaginationQueryFilter
    {
        /// <summary>
        /// cedula de identidad del usuario de EasyTrufi
        /// </summary>
        [SwaggerSchema("Identificador del usuario", Nullable = true)]
        public string? Cedula { get; set; }
    }
}
