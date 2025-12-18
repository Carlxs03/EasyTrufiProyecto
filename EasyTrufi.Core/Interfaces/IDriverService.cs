using EasyTrufi.Core.CustomEntities;
using EasyTrufi.Core.Entities;
using EasyTrufi.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Core.Interfaces
{
    public interface IDriverService
    {
        Task<IEnumerable<Driver>> GetAllDriversAsync();

        Task<Driver> GetDriverByIdAsync(long id);

        Task InsertDriverAsync(Driver driver);

        Task UpdateDriverAsync(Driver driver);

        Task DeleteDriverAsync(long id);

        Task<ResponseData> GetAllDriversAsync(DriverQueryFilter filters);
    }
}
