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
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public PaymentController(
            IPaymentService paymentService,
            IMapper mapper,
            IValidationService validationService
            )
        {
            _paymentService = paymentService;
            _mapper = mapper;
            _validationService = validationService;
        }

        #region Dto Mapper
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


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentDtoMapperId(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            var paymentDTO = _mapper.Map<PaymentDTO>(payment);

            var response = new ApiResponse<PaymentDTO>(paymentDTO);

            return Ok(response);
        }

        [HttpPost()]
        public async Task<IActionResult> InsertPaymentDtoMapper([FromBody] PaymentDTO paymentDTO)
        {
            var payment = _mapper.Map<Payment>(paymentDTO);
            await _paymentService.InsertPaymentAsync(payment);

            var response = new ApiResponse<Payment>(payment);

            return Ok(response);
        }

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

