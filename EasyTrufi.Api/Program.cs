using EasyTrufi.Core.CustomEntities;
using EasyTrufi.Core.Interfaces;
using EasyTrufi.Core.Services;
using EasyTrufi.Infraestructure.Data; // namespace donde esté AppDbContext
using EasyTrufi.Infraestructure.DTOs;
using EasyTrufi.Infraestructure.Filters;
using EasyTrufi.Infraestructure.Mappings;
using EasyTrufi.Infraestructure.Repositories;
using EasyTrufi.Infraestructure.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;

namespace EasyTrufi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Configurar configuración para diferentes entornos
            if (builder.Environment.IsDevelopment())
            {
                // En desarrollo: cargar User Secrets
                builder.Configuration.AddUserSecrets<Program>();
            }
            // En producción, los secrets vendrán de Variables de Entorno o Azure Key Vault


            //Configuracion base
            //builder.Configuration.Sources.Clear();
            //builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",
            //                     optional: true, reloadOnChange: true);






            #region Configurar la BD SqlServer
            var connectionString = builder.Configuration.GetConnectionString("ConnectionSqlServer");
            builder.Services.AddDbContext<EasyTrufiContext>(options => options.UseSqlServer(connectionString));
            #endregion

            // Registrar repo Inyectar Dependencias
            builder.Services.AddTransient<IUserRepository, UserRepository>();

            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            builder.Services.AddTransient<IUserService, UserService>();

            builder.Services.AddTransient<INfcCardService, NfcCardService>();
            builder.Services.AddTransient<IPaymentService, PaymentService>();
            builder.Services.AddTransient<IValidatorService, ValidatorService>();
            builder.Services.AddTransient<ITopupService, TopupService>();
            builder.Services.AddTransient<IDriverService, DriverService>();
            builder.Services.AddTransient<INfcCardRepository, NfcCardRepository>();


            //builder.Services.AddScoped<IUserRepository, UserRepository>();


            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            builder.Services.AddSingleton<IPasswordService, PasswordService>();


            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Registrar IDbConnectionFactory, UnitOfWork, DapperContext y repos
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            builder.Services.AddScoped<IDapperContext, DapperContext>();

            builder.Services.AddTransient<ISecurityService, SecurityService>();

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

            builder.Services.Configure<PasswordOptions>
                (builder.Configuration.GetSection("PasswordOptions"));




            // Configurar Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new()
                {
                    Title = "Backend EasyTrufi API",
                    Version = "v1",
                    Description = "Documentación de la API de EasyTrufi - .NET 8",
                    Contact = new()
                    {
                        Name = "Equipo de Desarrollo UCB",
                        Email = "carlos.camacho.h@ucb.edu.bo"
                    }
                });

                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                // Configurar para que muestre los parámetros de objetos complejos
                options.EnableAnnotations();


            });

            builder.Services.AddApiVersioning(options =>
            {
                // Reporta las versiones soportadas y obsoletas en encabezados de respuesta
                options.ReportApiVersions = true;

                // Versión por defecto si no se especifica
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);

                // Soporta versionado mediante URL, Header o QueryString
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),       // Ejemplo: /api/v1/...
                    new HeaderApiVersionReader("x-api-version"), // Ejemplo: Header ? x-api-version: 1.0
                    new QueryStringApiVersionReader("api-version") // Ejemplo: ?api-version=1.0
                );
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Authentication:Issuer"],
                    ValidAudience = builder.Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(
                            builder.Configuration["Authentication:SecretKey"]
                        )
                    )
                };
            });



            // FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();

            // Services
            builder.Services.AddScoped<IValidationService, ValidationService>();
           




            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Variables de entorno
            //builder.Configuration.AddEnvironmentVariables();


            var app = builder.Build();

            // Usar Swagger
            /*
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Social Media API v1");
                    options.RoutePrefix = string.Empty; // Swagger será accesible en la raíz
                });
                
            */
            //}

            app.UseSwagger();
            app.UseSwaggerUI();

            //if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
            app.UseHttpsRedirection();
            //app.UseAuthorization();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.Run();
        }
    }
}





