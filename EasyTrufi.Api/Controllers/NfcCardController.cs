using AutoMapper;
using EasyTrufi.Api.Responses;
using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Interfaces;
using EasyTrufi.Core.Services;
using EasyTrufi.Infraestructure.DTOs;
using EasyTrufi.Infraestructure.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet()]
        public async Task<IActionResult> GetCardsDtoMapper()
        {
            var cards = await _NfcCardService.GetAllCardsAsync();
            var cardsDto = _mapper.Map<IEnumerable<NfcCardDTO>>(cards);

            var response = new ApiResponse<IEnumerable<NfcCardDTO>>(cardsDto);

            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCardsDtoMapperId(int id)
        {
            var card = await _NfcCardService.GetCardByIdAsync(id);
            var cardDto = _mapper.Map<NfcCardDTO>(card);

            var response = new ApiResponse<NfcCardDTO>(cardDto);

            return Ok(response);
        }

        [HttpPost()]
        public async Task<IActionResult> InsertNfcCardDtoMapper([FromBody] NfcCardDTO cardDTO)
        {
            var card = _mapper.Map<NfcCard>(cardDTO);
            await _NfcCardService.InsertCardAsync(card);

            var response = new ApiResponse<NfcCard>(card);

            return Ok(response);
        }

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
