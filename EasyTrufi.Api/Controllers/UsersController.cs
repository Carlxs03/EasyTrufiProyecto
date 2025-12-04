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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace EasyTrufi.Api.Controllers
{



    //[Authorize]
    //[Produces("application/json")]
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

        /// <summary>
        /// Recupera una lista paginada de usuarios como objetos de transferencia de datos (DTO) según los filtros especificados.
        /// </summary>
        /// <remarks>Este método utiliza un mapeador para convertir las publicaciones recuperadas en DTO, que luego se 
        /// devuelven junto con la información de paginación. Si se produce un error durante el proceso, se devuelve un 
        /// código de estado 500 con los detalles del error.<see cref="ApiResponse{T}"/></remarks>
        /// <param name="filters">Los filtros que se aplicarán al recuperar publicaciones, como la paginación y los criterios de búsqueda.</param>
        /// <returns>Un <see cref="IActionResult"/> que contiene un <see cref="ApiResponse{T}"/> con una colección de objetos <see cref="UserDTO_GetAll"/> 
        /// y detalles de paginación.</returns>
        /// <response code="200">Retorna la lista de usuarios en formato DTO</response>
        /// <response code="500">Error interno del servidor</response>

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UserDTO>>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]

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

        /// <summary>
        /// Recupera un usuario específico por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del usuario.</param>
        /// <returns>Un <see cref="IActionResult"/> que contiene un <see cref="ApiResponse{T}"/> con los datos del usuario.</returns>
        /// <response code="200">Retorna el usuario solicitado</response>
        /// <response code="404">Usuario no encontrado</response>
        [HttpGet("{id}")]

        public async Task<IActionResult> GetUsersDtoMapperId(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            var userDto = _mapper.Map<UserDTO_GetAll>(user);

            var response = new ApiResponse<UserDTO_GetAll>(userDto);

            return Ok(response);
        }

        /// <summary>
        /// Inserta un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="userDTO">El objeto DTO que contiene los datos del usuario a insertar.</param>
        /// <returns>Un <see cref="IActionResult"/> que contiene un <see cref="ApiResponse{T}"/> con los datos del usuario creado.</returns>
        /// <response code="200">Usuario creado exitosamente</response>
        /// <response code="400">Datos inválidos para la creación del usuario</response>
        [HttpPost()]

        public async Task<IActionResult> InsertUserDtoMapper([FromBody] UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            await _userService.InsertUserAsync(user);

            var response = new ApiResponse<User>(user);

            return Ok(response);
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente.
        /// </summary>
        /// <param name="id">El identificador único del usuario a actualizar.</param>
        /// <param name="userDTO">El objeto DTO que contiene los nuevos datos del usuario.</param>
        /// <returns>Un <see cref="IActionResult"/> que contiene un <see cref="ApiResponse{T}"/> con los datos del usuario actualizado.</returns>
        /// <response code="200">Usuario actualizado exitosamente</response>
        /// <response code="404">Usuario no encontrado</response>
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

        /// <summary>
        /// Elimina un usuario del sistema.
        /// </summary>
        /// <param name="id">El identificador único del usuario a eliminar.</param>
        /// <returns>Un <see cref="IActionResult"/> que indica el resultado de la operación.</returns>
        /// <response code="204">Usuario eliminado exitosamente</response>
        /// <response code="404">Usuario no encontrado</response>
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
