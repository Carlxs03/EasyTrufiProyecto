using EasyTrufi.Core.Interfaces;
using EasyTrufi.Core.Services;
using EasyTrufi.Infraestructure.Data; // namespace donde esté AppDbContext
using EasyTrufi.Infraestructure.DTOs;
using EasyTrufi.Infraestructure.Filters;
using EasyTrufi.Infraestructure.Mappings;
using EasyTrufi.Infraestructure.Repositories;
using EasyTrufi.Infraestructure.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;

namespace EasyTrufi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configurar la BD SqlServer
            var connectionString = builder.Configuration.GetConnectionString("ConnectionSqlServer");
            builder.Services.AddDbContext<EasyTrufiContext>(options => options.UseSqlServer(connectionString));
            #endregion

            // Registrar repo
            builder.Services.AddTransient<IUserRepository, UserRepository>();

            builder.Services.AddTransient<IUserService, UserService>();

            builder.Services.AddTransient<INfcCardService, NfcCardService>();
            builder.Services.AddTransient<INfcCardRepository, NfcCardRepository>();


            builder.Services.AddAutoMapper(typeof(MappingProfile));

            //Validaciones
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });

            // FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();

            // Services
            builder.Services.AddScoped<IValidationService, ValidationService>();





            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}



