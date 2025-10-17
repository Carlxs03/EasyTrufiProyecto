using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Interfaces;

using EasyTrufi.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EasyTrufi.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EasyTrufiContext _context;

        public UserRepository(EasyTrufiContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(
                x => x.Id == id);
            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            User user = await GetUserByIdAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task InsertUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CedulaExists(string cedula)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Cedula==cedula);

            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> EmailExists(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Email==email);

            if(user != null)
            {
                return true ;
            }
            else
            {
                return false;
            }
        }
    }
}
