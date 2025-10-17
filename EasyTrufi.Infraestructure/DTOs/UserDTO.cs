using EasyTrufi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Infraestructure.DTOs
{
    public class UserDTO
    {
        //public long Id { get; set; }

        public string Cedula { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

        public string? Email { get; set; }

        public string PasswordHash { get; set; } = null!;

        //public bool EmailVerified { get; set; }
    }
}
