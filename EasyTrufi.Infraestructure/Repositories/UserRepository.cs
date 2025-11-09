using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Enum;
using EasyTrufi.Core.Interfaces;

using EasyTrufi.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EasyTrufi.Infraestructure.Repositories
{
    public class UserRepository : BaseRepository<User> , IUserRepository
    {
        private readonly EasyTrufiContext _context;
        private readonly IDapperContext _dapper;

        public UserRepository(EasyTrufiContext context
            , IDapperContext dapper) : base(context)
        {
            _context = context;
            _dapper = dapper;
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

        public async Task<IEnumerable<User>> GetAllUsersDapperAsync(int limit = 10)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => @"
                SELECT Id, Cedula, Full_Name, Date_Of_Birth, Email
                FROM users
                ORDER BY Created_At DESC
                OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;",

                    DatabaseProvider.MySql => @"
                SELECT Id, Cedula, Full_Name, Date_Of_Birth, Email
                FROM users
                ORDER BY Created_At DESC
                LIMIT @Limit;",
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<User>(sql, new { Limit = limit });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
