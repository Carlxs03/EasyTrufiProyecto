using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasyTrufi.Core.Entities;

namespace EasyTrufi.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>>GetAllUsersAsync();

        Task<User> GetUserByIdAsync(int id);

        Task InsertUserAsync (User user);

        Task UpdateUserAsync (User user);

        Task DeleteUserAsync (int id);

    }
}
