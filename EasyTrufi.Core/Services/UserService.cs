using EasyTrufi.Core.CustomEntities;
using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Interfaces;
using EasyTrufi.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Core.Services
{
    public class UserService : IUserService
    {

        //public readonly IBaseRepository<User> _userRepository;
        //public readonly IUserRepository _userRepository;

        public readonly IUnitOfWork _unitOfWork;

        public UserService(
            //IBaseRepository<User> userRepository
            //IUserRepository userRepository

            IUnitOfWork unitOfWork

            )
        {
            //_userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.userRepository.GetAllUserAsync();
            //return await _userRepository.GetAll();
            //return await _userRepository.GetAllUserAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.userRepository.GetUserByIdAsync(id);
            //return await _userRepository.GetById(id);
        }

        public async Task InsertUserAsync(User user)
        {
            await _unitOfWork.userRepository.InsertUserAsync(user);
            //await _userRepository.Add(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _unitOfWork.userRepository.UpdateUserAsync(user);

            //await _userRepository.Update(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _unitOfWork.userRepository.DeleteUserAsync(id);

            //await _userRepository.Delete(id);
        }

        public async Task<Boolean> CedulaExists(string id)
        {
            return await _unitOfWork.userRepository.CedulaExists(id);
        }

        public async Task<ResponseData> GetAllUsersAsync(UserQueryFilter filters)
        {
            var users = await _unitOfWork.userRepository.GetAllUserAsync();

            if (filters.Cedula != null)
            {
                users = users.Where(x => x.Cedula == filters.Cedula);
            }

            var pagedPosts = PagedList<object>.Create(users, filters.PageNumber, filters.PageSize);
            if (pagedPosts.Any())
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de users recuperados correctamente" } },
                    Pagination = pagedPosts,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedPosts,
                    StatusCode = HttpStatusCode.OK
                };
            }

        }
    }
}
