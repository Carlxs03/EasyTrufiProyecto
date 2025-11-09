using EasyTrufi.Core.Interfaces;
using EasyTrufi.Core.Services;
using EasyTrufi.Infraestructure.Data; // namespace donde esté AppDbContext
using EasyTrufi.Infraestructure.DTOs;
using EasyTrufi.Infraestructure.Filters;
using EasyTrufi.Infraestructure.Mappings;
using EasyTrufi.Infraestructure.Repositories;
using EasyTrufi.Infraestructure.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Connections;
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

            // Registrar repo Inyectar Dependencias
            //builder.Services.AddTransient<IUserRepository, UserRepository>();

            builder.Services.AddTransient<IUserService, UserService>();

            builder.Services.AddTransient<INfcCardService, NfcCardService>();
            builder.Services.AddTransient<IPaymentService, IPaymentService>();
            //builder.Services.AddTransient<INfcCardRepository, NfcCardRepository>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));


            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Registrar IDbConnectionFactory, UnitOfWork, DapperContext y repos
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            builder.Services.AddScoped<IDapperContext, DapperContext>();


            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

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



