using AutoMapper;
using EasyTrufi.Api.Responses;
using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Interfaces;
using EasyTrufi.Infraestructure.DTOs;
using EasyTrufi.Infraestructure.Validators;
using Microsoft.AspNetCore.Mvc;


namespace EasyTrufi.Api.Controllers
{
    


    [Route("api/[controller]")]
    [ApiController]

    


    public class UsersController : ControllerBase
    {

        private readonly IValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IValidationService validationService
            )
        {
            _userService = userService;
            _mapper = mapper;
            _validationService = validationService;
        }

        #region Dto Mapper
        [HttpGet()]
        public async Task<IActionResult> GetUsersDtoMapper()
        {
            var users = await _userService.GetAllUsersAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDTO_GetAll>>(users);

            var response = new ApiResponse<IEnumerable<UserDTO_GetAll>>(usersDto);

            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsersDtoMapperId(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            var userDto = _mapper.Map<UserDTO_GetAll>(user);

            var response = new ApiResponse<UserDTO_GetAll>(userDto);

            return Ok(response);
        }

        [HttpPost()]
        public async Task<IActionResult> InsertUserDtoMapper([FromBody] UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            await _userService.InsertUserAsync(user);

            var response = new ApiResponse<User>(user);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserDtoMapper(int id,
            [FromBody] UserDTO userDTO)
        {

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("Post no encontrado");

            _mapper.Map(userDTO, user);
            await _userService.UpdateUserAsync(user);

            var response = new ApiResponse<User>(user);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserDtoMapper(int id)
        {
            var post = await _userService.GetUserByIdAsync(id);
            if (post == null)
                return NotFound("User no encontrado");

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        #endregion


    }
}
