using AutoMapper;
using EasyTrufi.Api.Responses;
using EasyTrufi.Core.CustomEntities;
using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Enum;
using EasyTrufi.Core.Interfaces;
using EasyTrufi.Core.QueryFilters;
using EasyTrufi.Core.QueryFilters;
using EasyTrufi.Infraestructure.DTOs;
using EasyTrufi.Infraestructure.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

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
        public async Task<IActionResult> GetUsersDtoMapper(
            [FromQuery]UserQueryFilter filters )
        {
            try
            {
                var users = await _userService.GetAllUsersAsync(filters);

                var usersDto = _mapper.Map<IEnumerable<UserDTO_GetAll>>(users.Pagination);

                var pagination = new Pagination
                {
                    TotalCount = users.Pagination.TotalCount,
                    PageSize = users.Pagination.PageSize,
                    CurrentPage = users.Pagination.CurrentPage,
                    TotalPages = users.Pagination.TotalPages,
                    HasNextPage = users.Pagination.HasNextPage,
                    HasPreviousPage = users.Pagination.HasPreviousPage
                };
                var response = new ApiResponse<IEnumerable<UserDTO_GetAll>>(usersDto)
                {
                    Pagination = pagination,
                    Messages = users.Messages
                };

                return StatusCode((int)users.StatusCode, response);
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
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User no encontrado");

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        #endregion


    }
}
