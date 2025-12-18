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
using System.Net;

namespace EasyTrufi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        private readonly INfcCardRepository _nfcCardService;
        private readonly ITopupService _topupService;

        public PaymentController(
            IPaymentService paymentService,
            IMapper mapper,
            IValidationService validationService,
            INfcCardRepository nfcCardService,
            ITopupService topupService
            )
        {
            _paymentService = paymentService;
            _mapper = mapper;
            _validationService = validationService;
            _nfcCardService = nfcCardService;
            _topupService = topupService;
        }

        #region Dto Mapper
        /// <summary>
        /// Recupera una lista paginada de pagos realizados en el sistema.
        /// </summary>
        /// <param name="filters">Filtros aplicados para la búsqueda de pagos, como paginación y criterios específicos.</param>
        /// <returns>Un <see cref="IActionResult"/> que contiene un <see cref="ApiResponse{T}"/> con una colección de objetos <see cref="PaymentDTO"/> y detalles de paginación.</returns>
        /// <response code="200">Retorna la lista de pagos</response>
        /// <response code="500">Error interno del servidor</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PaymentDTO>>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet()]

        public async Task<IActionResult> GetPaymentsDtoMapper(
            [FromQuery] PaymentQueryFilter filters)
        {
            try
            {
                var payments = await _paymentService.GetAllPaymentsAsync(filters); 

                var paymentsDto = _mapper.Map<IEnumerable<PaymentDTO>>(payments.Pagination);

                var pagination = new Pagination
                {
                    TotalCount = payments.Pagination.TotalCount,
                    PageSize = payments.Pagination.PageSize,
                    CurrentPage = payments.Pagination.CurrentPage,
                    TotalPages = payments.Pagination.TotalPages,
                    HasNextPage = payments.Pagination.HasNextPage,
                    HasPreviousPage = payments.Pagination.HasPreviousPage
                };
                var response = new ApiResponse<IEnumerable<PaymentDTO>>(paymentsDto)
                {
                    Pagination = pagination,
                    Messages = payments.Messages
                };

                return StatusCode((int)payments.StatusCode, response);
            }
            catch (Exception err)
            {
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = err.Message } },
                };
                return StatusCode(500, responsePost);
            }

        }


        /// <summary>
        /// Recupera un pago específico por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del pago.</param>
        /// <returns>Un <see cref="IActionResult"/> que contiene un <see cref="ApiResponse{T}"/> con los datos del pago.</returns>
        /// <response code="200">Retorna el pago solicitado</response>
        /// <response code="404">Pago no encontrado</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<PaymentDTO>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{id}")]

        public async Task<IActionResult> GetPaymentDtoMapperId(long id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            var paymentDTO = _mapper.Map<PaymentDTO>(payment);

            var response = new ApiResponse<PaymentDTO>(paymentDTO);

            return Ok(response);
        }

        /// <summary>
        /// Inserta un nuevo pago en el sistema.
        /// </summary>
        /// <param name="paymentDTO">El objeto DTO que contiene los datos del pago a insertar.</param>
        /// <returns>Un <see cref="IActionResult"/> que contiene un <see cref="ApiResponse{T}"/> con los datos del pago creado.</returns>
        /// <response code="200">Pago creado exitosamente</response>
        /// <response code="400">Datos inválidos para la creación del pago</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Payment>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost()]

        public async Task<IActionResult> InsertPaymentDtoMapper([FromBody] PaymentDTO paymentDTO)
        {
            // Verificar que la tarjeta NFC exista y esté activa
            var nfcCard = await _nfcCardService.GetCardByIdAsync(paymentDTO.NfcCardId);
            if (nfcCard == null)
                return BadRequest("La tarjeta NFC no existe.");
            if (!nfcCard.Active)
                return BadRequest("La tarjeta NFC no está activa.");

            // Calcular el saldo de la tarjeta
            var allTopups = await _topupService.GetAllTopupsAsync();
            var totalTopupCents = allTopups
                .Where(t => t.NfcCardId == paymentDTO.NfcCardId)
                .Sum(t => t.AmountCents);

            var allPayments = await _paymentService.GetAllPaymentsAsync();
            var totalPaymentCents = allPayments
                .Where(p => p.NfcCardId == paymentDTO.NfcCardId)
                .Sum(p => p.AmountCents);

            var balanceCents = totalTopupCents - totalPaymentCents;

            // Verificar que el saldo sea suficiente para realizar el pago
            if (balanceCents < paymentDTO.AmountCents)
                return BadRequest("Saldo insuficiente para realizar el pago.");

            // Mapear el DTO a la entidad y realizar el pago
            var payment = _mapper.Map<Payment>(paymentDTO);
            await _paymentService.InsertPaymentAsync(payment);

            var response = new ApiResponse<Payment>(payment);

            return Ok(response);
        }

        /// <summary>
        /// Actualiza los datos de un pago existente.
        /// </summary>
        /// <param name="id">El identificador único del pago a actualizar.</param>
        /// <param name="paymentDTO">El objeto DTO que contiene los nuevos datos del pago.</param>
        /// <returns>Un <see cref="IActionResult"/> que contiene un <see cref="ApiResponse{T}"/> con los datos del pago actualizado.</returns>
        /// <response code="200">Pago actualizado exitosamente</response>
        /// <response code="404">Pago no encontrado</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Payment>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPut("{id}")]

        public async Task<IActionResult> UpdatePaymentDtoMapper(int id,
            [FromBody] PaymentDTO paymentDTO)
        {

            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound("payment no encontrado");

            _mapper.Map(paymentDTO, payment);
            await _paymentService.UpdatePaymentAsync(payment);

            var response = new ApiResponse<Payment>(payment);

            return Ok(response);
        }


        /// <summary>
        /// Elimina un pago del sistema.
        /// </summary>
        /// <param name="id">El identificador único del pago a eliminar.</param>
        /// <returns>Un <see cref="IActionResult"/> que indica el resultado de la operación.</returns>
        /// <response code="204">Pago eliminado exitosamente</response>
        /// <response code="404">Pago no encontrado</response>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeletePaymentDtoMapper(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound("Payment no encontrado");

            await _paymentService.DeletePaymentAsync(id);
            return NoContent();
        }

        #endregion


    }


}

