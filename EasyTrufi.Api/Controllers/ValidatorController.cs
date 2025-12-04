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
using System.Net;

namespace EasyTrufi.Api.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con los validadores.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValidatorController : ControllerBase
    {
        private readonly IValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly IValidatorService _validatorService;

        /// <summary>
        /// Constructor del controlador ValidatorController.
        /// </summary>
        /// <param name="validatorService">Servicio para gestionar validadores.</param>
        /// <param name="mapper">Instancia de AutoMapper para realizar mapeos entre entidades y DTOs.</param>
        /// <param name="validationService">Servicio para realizar validaciones.</param>
        public ValidatorController(
            IValidatorService validatorService,
            IMapper mapper,
            IValidationService validationService
            )
        {
            _validatorService = validatorService;
            _mapper = mapper;
            _validationService = validationService;
        }

        #region Dto Mapper

        /// <summary>
        /// Obtiene una lista de validadores con soporte para paginación y filtros.
        /// </summary>
        /// <param name="filters">Filtros de consulta para los validadores.</param>
        /// <returns>Una lista de validadores en formato DTO con información de paginación.</returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllValidatorsDtoMapper(
            [FromQuery] ValidatorsQueryFilters filters)
        {
            try
            {
                var validators = await _validatorService.GetAllValidatorsAsync(filters);

                var validatorsDto = _mapper.Map<IEnumerable<ValidatorDTO>>(validators.Pagination);

                var pagination = new Pagination
                {
                    TotalCount = validators.Pagination.TotalCount,
                    PageSize = validators.Pagination.PageSize,
                    CurrentPage = validators.Pagination.CurrentPage,
                    TotalPages = validators.Pagination.TotalPages,
                    HasNextPage = validators.Pagination.HasNextPage,
                    HasPreviousPage = validators.Pagination.HasPreviousPage
                };
                var response = new ApiResponse<IEnumerable<ValidatorDTO>>(validatorsDto)
                {
                    Pagination = pagination,
                    Messages = validators.Messages
                };

                return StatusCode((int)validators.StatusCode, response);
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
        /// Obtiene un validador específico por su identificador.
        /// </summary>
        /// <param name="id">Identificador único del validador.</param>
        /// <returns>El validador correspondiente en formato DTO.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValidatorsDtoMapperId(long id)
        {
            var validator = await _validatorService.GetValidatorByIdAsync(id);
            var validatorDto = _mapper.Map<ValidatorDTO>(validator);

            var response = new ApiResponse<ValidatorDTO>(validatorDto);

            return Ok(response);
        }

        /// <summary>
        /// Inserta un nuevo validador en el sistema.
        /// </summary>
        /// <param name="validatorDto">DTO con la información del validador a insertar.</param>
        /// <returns>El validador insertado.</returns>
        [HttpPost()]
        public async Task<IActionResult> InsertValidatorDtoMapper([FromBody] ValidatorDTO validatorDto)
        {
            var validator = _mapper.Map<Validator>(validatorDto);
            await _validatorService.InsertValidatorAsync(validator);

            var response = new ApiResponse<Validator>(validator);

            return Ok(response);
        }

        /// <summary>
        /// Actualiza la información de un validador existente.
        /// </summary>
        /// <param name="id">Identificador único del validador a actualizar.</param>
        /// <param name="validatorDto">DTO con la nueva información del validador.</param>
        /// <returns>El validador actualizado.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateValidatorDtoMapper(long id,
            [FromBody] ValidatorDTO validatorDto)
        {
            var validator = await _validatorService.GetValidatorByIdAsync(id);
            if (validator == null)
                return NotFound("Validador no encontrado");

            _mapper.Map(validatorDto, validator);
            await _validatorService.UpdateValidatorAsync(validator);

            var response = new ApiResponse<Validator>(validator);

            return Ok(response);
        }

        /// <summary>
        /// Elimina un validador del sistema.
        /// </summary>
        /// <param name="id">Identificador único del validador a eliminar.</param>
        /// <returns>Respuesta sin contenido si la operación es exitosa.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValidatorDtoMapper(long id)
        {
            var validator = await _validatorService.GetValidatorByIdAsync(id);
            if (validator == null)
                return NotFound("validador no encontrado");

            await _validatorService.DeleteValidatorAsync(id);
            return NoContent();
        }

        #endregion
    }
}
