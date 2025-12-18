using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Infraestructure.DTOs
{
    public class DriverDTO
    {
        //public long Id { get; set; }

        public string Cedula { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public bool Active { get; set; }

    }
}
