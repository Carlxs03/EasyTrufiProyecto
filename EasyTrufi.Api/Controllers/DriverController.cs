using AutoMapper;
using EasyTrufi.Api.Responses;
using EasyTrufi.Core.CustomEntities;
using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Enum;
using EasyTrufi.Core.Interfaces;
using EasyTrufi.Core.QueryFilters;
using EasyTrufi.Infraestructure.DTOs;
using EasyTrufi.Infraestructure.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyTrufi.Api.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con los conductores (Drivers).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly IDriverService _driverService;

        /// <summary>
        /// Constructor del controlador DriverController.
        /// </summary>
        /// <param name="driverService">Servicio para gestionar los conductores.</param>
        /// <param name="mapper">Instancia de AutoMapper para realizar mapeos entre entidades y DTOs.</param>
        /// <param name="validationService">Servicio para realizar validaciones.</param>
        public DriverController(
            IDriverService driverService,
            IMapper mapper,
            IValidationService validationService
            )
        {
            _driverService = driverService;
            _mapper = mapper;
            _validationService = validationService;
        }

        #region Dto Mapper

        /// <summary>
        /// Obtiene una lista de conductores con soporte para paginación y filtros.
        /// </summary>
        /// <param name="filters">Filtros de consulta para los conductores.</param>
        /// <returns>Una lista de conductores en formato DTO con información de paginación.</returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllDriversDtoMapper(
            [FromQuery] DriverQueryFilter filters)
        {
            try
            {
                var drivers = await _driverService.GetAllDriversAsync(filters);

                var driversDto = _mapper.Map<IEnumerable<DriverDTO>>(drivers.Pagination);

                var pagination = new Pagination
                {
                    TotalCount = drivers.Pagination.TotalCount,
                    PageSize = drivers.Pagination.PageSize,
                    CurrentPage = drivers.Pagination.CurrentPage,
                    TotalPages = drivers.Pagination.TotalPages,
                    HasNextPage = drivers.Pagination.HasNextPage,
                    HasPreviousPage = drivers.Pagination.HasPreviousPage
                };
                var response = new ApiResponse<IEnumerable<DriverDTO>>(driversDto)
                {
                    Pagination = pagination,
                    Messages = drivers.Messages
                };

                return StatusCode((int)drivers.StatusCode, response);
            }
            catch (Exception err)
            {
                var responseValidator = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = err.Message } },
                };
                return StatusCode(500, responseValidator);
            }
        }

        /// <summary>
        /// Obtiene un conductor específico por su identificador.
        /// </summary>
        /// <param name="id">Identificador único del conductor.</param>
        /// <returns>El conductor correspondiente en formato DTO.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriverDtoMapperId(long id)
        {
            var driver = await _driverService.GetDriverByIdAsync(id);
            var driverDto = _mapper.Map<DriverDTO>(driver);

            var response = new ApiResponse<DriverDTO>(driverDto);

            return Ok(response);
        }

        /// <summary>
        /// Inserta un nuevo conductor en el sistema.
        /// </summary>
        /// <param name="driverDto">DTO con la información del conductor a insertar.</param>
        /// <returns>El conductor insertado.</returns>
        [HttpPost()]
        public async Task<IActionResult> InsertDriverDtoMapper([FromBody] DriverDTO driverDto)
        {
            var driver = _mapper.Map<Driver>(driverDto);
            await _driverService.InsertDriverAsync(driver);

            var response = new ApiResponse<Driver>(driver);

            return Ok(response);
        }

        /// <summary>
        /// Actualiza la información de un conductor existente.
        /// </summary>
        /// <param name="id">Identificador único del conductor a actualizar.</param>
        /// <param name="driverDto">DTO con la nueva información del conductor.</param>
        /// <returns>El conductor actualizado.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriverDtoMapper(long id,
            [FromBody] DriverDTO driverDto)
        {
            var driver = await _driverService.GetDriverByIdAsync(id);
            if (driver == null)
                return NotFound("Conductor no encontrado");

            _mapper.Map(driverDto, driver);
            await _driverService.UpdateDriverAsync(driver);

            var response = new ApiResponse<Driver>(driver);

            return Ok(response);
        }

        /// <summary>
        /// Elimina un conductor del sistema.
        /// </summary>
        /// <param name="id">Identificador único del conductor a eliminar.</param>
        /// <returns>Respuesta sin contenido si la operación es exitosa.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverDtoMapper(long id)
        {
            var driver = await _driverService.GetDriverByIdAsync(id);
            if (driver == null)
                return NotFound("Conductor no encontrado");

            await _driverService.DeleteDriverAsync(id);
            return NoContent();
        }

        #endregion
    }
}
