using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Interfaces;

namespace EasyTrufi.Core.Services
{
    public class UserService : IUserService
    {

        public readonly IUserRepository _userRepository;
        public UserService(
            IUserRepository userRepository
            )
        {
            _userRepository = userRepository;
        }


        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUserAsync();
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);  
        }

        public async Task InsertUserAsync(User user)
        {
            await _userRepository.InsertUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<Boolean> CedulaExists(string id)
        {
            return await _userRepository.CedulaExists(id);
        }
    }
}
