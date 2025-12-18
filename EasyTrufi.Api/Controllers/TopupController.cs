using AutoMapper;
using EasyTrufi.Api.Responses;
using EasyTrufi.Core.CustomEntities;
using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Enum;
using EasyTrufi.Core.Interfaces;
using EasyTrufi.Core.QueryFilters;
using EasyTrufi.Core.Services;
using EasyTrufi.Infraestructure.DTOs;
using EasyTrufi.Infraestructure.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyTrufi.Api.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con las recargas (Topups).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TopupController : ControllerBase
    {
        private readonly IValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly ITopupService _topupService;
        private readonly IUserService _userService;
        private readonly INfcCardService _nfcCardService;

        /// <summary>
        /// Constructor del controlador TopupController.
        /// </summary>
        /// <param name="topupService">Servicio para gestionar las recargas.</param>
        /// <param name="mapper">Instancia de AutoMapper para realizar mapeos entre entidades y DTOs.</param>
        /// <param name="validationService">Servicio para realizar validaciones.</param>
        public TopupController(
            ITopupService topupService,
            IMapper mapper,
            IValidationService validationService,
            IUserService userService,
            INfcCardService nfcCardService
            )
        {
            _topupService = topupService;
            _mapper = mapper;
            _validationService = validationService;
            _userService = userService;
            _nfcCardService = nfcCardService;
        }

        #region Dto Mapper

        /// <summary>
        /// Obtiene una lista de recargas con soporte para paginación y filtros.
        /// </summary>
        /// <param name="filters">Filtros de consulta para las recargas.</param>
        /// <returns>Una lista de recargas en formato DTO con información de paginación.</returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllTopupsDtoMapper(
            //[FromQuery] TopupQueryFilter filters
            )
        {
            /*
            try
            {
                var topups = await _topupService.GetAllTopupsAsync(filters);

                var topupsDto = _mapper.Map<IEnumerable<TopupDTO>>(topups.Pagination);

                var pagination = new Pagination
                {
                    TotalCount = topups.Pagination.TotalCount,
                    PageSize = topups.Pagination.PageSize,
                    CurrentPage = topups.Pagination.CurrentPage,
                    TotalPages = topups.Pagination.TotalPages,
                    HasNextPage = topups.Pagination.HasNextPage,
                    HasPreviousPage = topups.Pagination.HasPreviousPage
                };
                var response = new ApiResponse<IEnumerable<TopupDTO>>(topupsDto)
                {
                    Pagination = pagination,
                    Messages = topups.Messages
                };

                return StatusCode((int)topups.StatusCode, response);
            }
            catch (Exception err)
            {
                var responseValidator = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = err.Message } },
                };
                return StatusCode(500, responseValidator);
            }
            */

            var topups = await _topupService.GetAllTopupsAsync();
            var topupsdto = _mapper.Map<IEnumerable<TopupDTO>>(topups);

            var response = new ApiResponse<IEnumerable<TopupDTO>>(topupsdto);

            return Ok(response);
        }

        /// <summary>
        /// Obtiene una recarga específica por su identificador.
        /// </summary>
        /// <param name="id">Identificador único de la recarga.</param>
        /// <returns>La recarga correspondiente en formato DTO.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopupDtoMapperId(long id)
        {
            var topup = await _topupService.GetTopupByIdAsync(id);
            var topupsDto = _mapper.Map<TopupDTO>(topup);

            var response = new ApiResponse<TopupDTO>(topupsDto);

            return Ok(response);
        }

        /// <summary>
        /// Inserta una nueva recarga en el sistema.
        /// </summary>
        /// <param name="topupsDto">DTO con la información de la recarga a insertar.</param>
        /// <returns>La recarga insertada.</returns>
        [HttpPost()]
        public async Task<IActionResult> InsertTopupDtoMapper([FromBody] TopupDTO topupsDto)
        {
            var nfccard = await _nfcCardService.GetCardByIdAsync(topupsDto.NfcCardId);
            if (nfccard == null)
                return NotFound("Tarjeta NFC no encontrada");
            if (!nfccard.Active)
                return BadRequest("La tarjeta NFC está inactiva");

            if (topupsDto.UserId != null)
            {
                var user = await _userService.GetUserByIdAsync(topupsDto.UserId.Value);
                if (user == null)
                    return NotFound("Usuario no encontrado");

                if (!user.EmailVerified)
                    return BadRequest("El usuario está inactivo, no ha verificado su Email");
            }


            var topup = _mapper.Map<Topup>(topupsDto);

            topup.Status = "COMPLETE";
            topup.Reference = null;

            await _topupService.InsertTopupAsync(topup);

            /*

            var topup1 = _mapper.Map<Topup>(topupsDto);

            await _userService.AddTopupAsync(topup1.UserId.Value, topup1);

            var topup2 = _mapper.Map<Topup>(topupsDto);

            await _nfcCardService.AddTopupAsync(topup2.NfcCardId, topup2);
            */
            

            var response = new ApiResponse<Topup>(topup);

            return Ok(response);
        }

        /// <summary>
        /// Actualiza la información de una recarga existente.
        /// </summary>
        /// <param name="id">Identificador único de la recarga a actualizar.</param>
        /// <param name="topupsDto">DTO con la nueva información de la recarga.</param>
        /// <returns>La recarga actualizada.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopupDtoMapper(long id,
            [FromBody] TopupDTO topupsDto)
        {
            var topup = await _topupService.GetTopupByIdAsync(id);
            if (topup == null)
                return NotFound("Recarga no encontrada");

            _mapper.Map(topupsDto, topup);
            await _topupService.UpdateTopupAsync(topup);

            var response = new ApiResponse<Topup>(topup);

            return Ok(response);
        }

        /// <summary>
        /// Elimina una recarga del sistema.
        /// </summary>
        /// <param name="id">Identificador único de la recarga a eliminar.</param>
        /// <returns>Respuesta sin contenido si la operación es exitosa.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopupDtoMapper(long id)
        {
            var topup = await _topupService.GetTopupByIdAsync(id);
            if (topup == null)
                return NotFound("Recarga no encontrada");

            await _topupService.DeleteTopupAsync(id);
            return NoContent();
        }

        #endregion
    }
}

