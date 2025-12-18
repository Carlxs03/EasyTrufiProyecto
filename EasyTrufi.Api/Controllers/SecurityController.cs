using AutoMapper;
using EasyTrufi.Api.Responses;
using EasyTrufi.Core.Interfaces;
using EasyTrufi.Core.Services;
using EasyTrufi.Infraestructure.Data;
using EasyTrufi.Infraestructure.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyTrufi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public SecurityController(ISecurityService securityService,
            IMapper mapper,
            IPasswordService passwordService)
        {
            _securityService = securityService;
            _mapper = mapper;
            _passwordService = passwordService;
        }

    


    [HttpPost]
        public async Task<IActionResult> User(SecurityDTO securityDto)
        {
            /*
            var security = _mapper.Map<Security>(securityDto);
            security.Password = _passwordService.Hash(security.Password);
            await _securityService.RegisterUser(security);

            securityDto = _mapper.Map<SecurityDTO>(security);
            var response = new ApiResponse<SecurityDTO>(securityDto);
            return Ok(response);
            */

            try
            {
                var security = _mapper.Map<Security>(securityDto);
                security.Password = _passwordService.Hash(security.Password);

                // Verificación rápida antes de guardar
                if (security.Role.ToString().Length > 15)
                {
                    return BadRequest("El nombre del ROL es demasiado largo para la base de datos (Máx 15).");
                }

                await _securityService.RegisterUser(security);

                securityDto = _mapper.Map<SecurityDTO>(security);
                var response = new ApiResponse<SecurityDTO>(securityDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Esto nos dará el error real de SQL
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, new { message = innerMessage, detail = ex.ToString() });
            }

        }
    }

}
