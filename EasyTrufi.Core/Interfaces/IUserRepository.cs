using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasyTrufi.Core.Entities;

namespace EasyTrufi.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User>GetUserByIdAsync(int id);
        Task InsertUserAsync(User user);

        Task UpdateUserAsync(User user);    

        Task DeleteUserAsync(int id);

        Task<bool> CedulaExists(string id);

        Task<bool> EmailExists(string email);
    }
}
