using AutoMapper;
using EasyTrufi.Api.Responses;
using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Interfaces;
using EasyTrufi.Core.Services;
using EasyTrufi.Infraestructure.DTOs;
using EasyTrufi.Infraestructure.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EasyTrufi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NfcCardController : ControllerBase
    {
        private readonly IValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly INfcCardService _NfcCardService;

        public NfcCardController(
            INfcCardService nfcCardService,
            IMapper mapper,
            IValidationService validationService
            )
        {
            _NfcCardService = nfcCardService;
            _mapper = mapper;
            _validationService = validationService;
        }

        #region Dto Mapper
        /// <summary>
        /// Recupera una lista de todas las tarjetas NFC registradas en el sistema.
        /// </summary>
        /// <returns>Un <see cref="IActionResult"/> que contiene un <see cref="ApiResponse{T}"/> con una colección de objetos <see cref="NfcCardDTO"/>.</returns>
        /// <response code="200">Retorna la lista de tarjetas NFC</response>
        /// <response code="500">Error interno del servidor</response>
        /// 
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<NfcCardDTO>>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet()]


        public async Task<IActionResult> GetCardsDtoMapper()
        {
            var cards = await _NfcCardService.GetAllCardsAsync();
            var cardsDto = _mapper.Map<IEnumerable<NfcCardDTO>>(cards);

            var response = new ApiResponse<IEnumerable<NfcCardDTO>>(cardsDto);

            return Ok(response);
        }

        /// <summary>
        /// Recupera una tarjeta NFC específica por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único de la tarjeta NFC.</param>
        /// <returns>Un <see cref="IActionResult"/> que contiene un <see cref="ApiResponse{T}"/> con los datos de la tarjeta NFC.</returns>
        /// <response code="200">Retorna la tarjeta NFC solicitada</response>
        /// <response code="404">Tarjeta NFC no encontrada</response>

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<NfcCardDTO>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{id}")]


        public async Task<IActionResult> GetCardsDtoMapperId(int id)
        {
            var card = await _NfcCardService.GetCardByIdAsync(id);
            var cardDto = _mapper.Map<NfcCardDTO>(card);

            var response = new ApiResponse<NfcCardDTO>(cardDto);

            return Ok(response);
        }

        /// <summary>
        /// Inserta una nueva tarjeta NFC en el sistema.
        /// </summary>
        /// <param name="cardDTO">El objeto DTO que contiene los datos de la tarjeta NFC a insertar.</param>
        /// <returns>Un <see cref="IActionResult"/> que contiene un <see cref="ApiResponse{T}"/> con los datos de la tarjeta NFC creada.</returns>
        /// <response code="200">Tarjeta NFC creada exitosamente</response>
        /// <response code="400">Datos inválidos para la creación de la tarjeta NFC</response>

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<NfcCard>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost()]


        public async Task<IActionResult> InsertNfcCardDtoMapper([FromBody] NfcCardDTO cardDTO)
        {
            var card = _mapper.Map<NfcCard>(cardDTO);
            await _NfcCardService.InsertCardAsync(card);

            var response = new ApiResponse<NfcCard>(card);

            return Ok(response);
        }

        /// <summary>
        /// Actualiza los datos de una tarjeta NFC existente.
        /// </summary>
        /// <param name="id">El identificador único de la tarjeta NFC a actualizar.</param>
        /// <param name="cardDTO">El objeto DTO que contiene los nuevos datos de la tarjeta NFC.</param>
        /// <returns>Un <see cref="IActionResult"/> que contiene un <see cref="ApiResponse{T}"/> con los datos de la tarjeta NFC actualizada.</returns>
        /// <response code="200">Tarjeta NFC actualizada exitosamente</response>
        /// <response code="404">Tarjeta NFC no encontrada</response>

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<NfcCard>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPut("{id}")]


        public async Task<IActionResult> UpdateCardDtoMapper(int id,
            [FromBody] NfcCardDTO cardDTO)
        {

            var card = await _NfcCardService.GetCardByIdAsync(id);
            if (card == null)
                return NotFound("Nfc Card no encontrado");

            _mapper.Map(cardDTO, card);
            await _NfcCardService.UpdateCardAsync(card);

            var response = new ApiResponse<NfcCard>(card);

            return Ok(response);
        }

        /// <summary>
        /// Elimina una tarjeta NFC del sistema.
        /// </summary>
        /// <param name="id">El identificador único de la tarjeta NFC a eliminar.</param>
        /// <returns>Un <see cref="IActionResult"/> que indica el resultado de la operación.</returns>
        /// <response code="204">Tarjeta NFC eliminada exitosamente</response>
        /// <response code="404">Tarjeta NFC no encontrada</response>

        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]


        public async Task<IActionResult> DeleteCardDtoMapper(int id)
        {
            var post = await _NfcCardService.GetCardByIdAsync(id);
            if (post == null)
                return NotFound("card no encontrado");

            await _NfcCardService.DeleteCardAsync(id);
            return NoContent();
        }

        #endregion

    }
}
