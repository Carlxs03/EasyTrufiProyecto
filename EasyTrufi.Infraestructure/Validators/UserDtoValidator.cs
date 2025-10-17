using EasyTrufi.Core.Interfaces;
using EasyTrufi.Core.Services;
using EasyTrufi.Infraestructure.DTOs;
using FluentValidation;
using FluentValidation.Internal;
using System.Globalization;
using System.Runtime.ConstrainedExecution;

namespace EasyTrufi.Infraestructure.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDTO>
    {
        private readonly IUserRepository _userRepository;

        public UserDtoValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;



        //Para la cedula de identidad
        RuleFor(x => x.Cedula)
                .Matches(@"^\d{8,8}$").WithMessage("La cedula de identidad debe tener 8 digitos y solo números")
                .NotEmpty().WithMessage("La cedula de identidad es obligatoria")
                .MustAsync(CedulaUnique).WithMessage("La cedula de identidad debe ser unica");

            //Para el nombre
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$").WithMessage("El nombre solo debe contener caracteres y espacios")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres");

            //Para el email
            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email)
                    .EmailAddress().WithMessage("Formato de correo electrónico inválido.")
                    .MustAsync(EmailUnique).WithMessage("El email ya esta registrado para otro usuario.");
            });

            //Para la contraseña
            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")
                .WithMessage("La contraseña debe tener al menos 8 caracteres, incluyendo mayúscula, minúscula y un número.");
        }

        private async Task<bool> EmailUnique(string email, CancellationToken token)
        {
            return !await _userRepository.EmailExists(email);
        }

        private async Task<bool> CedulaUnique(string ced, CancellationToken token)
        {
            return !await _userRepository.CedulaExists(ced);
        }


    }
}
