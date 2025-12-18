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
    public class DriverService : IDriverService
    {
        public readonly IUnitOfWork _unitOfWork;

        public DriverService
        (

            IUnitOfWork unitOfWork

        )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteDriverAsync(long id)
        {
            await _unitOfWork.driverRepository.Delete(id);
        }

        public async Task<IEnumerable<Driver>> GetAllDriversAsync()
        {
            return await _unitOfWork.driverRepository.GetAll();
        }

        public async Task<ResponseData> GetAllDriversAsync(DriverQueryFilter filters)
        {
            var drivers = await _unitOfWork.driverRepository.GetAll();

            if (filters.FullName != null)
            {
                drivers = drivers.Where(x => x.FullName == filters.FullName);
            }

            if (filters.Cedula != null)
            {
                drivers = drivers.Where(x => x.Cedula == filters.Cedula);
            }


            if (filters.Active != null)
            {
                drivers = drivers.Where(x => x.Active == filters.Active);
            }



            var pagedDrivers = PagedList<object>.Create(drivers, filters.PageNumber, filters.PageSize);
            if (pagedDrivers.Any())
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de users recuperados correctamente" } },
                    Pagination = pagedDrivers,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedDrivers,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<Driver> GetDriverByIdAsync(long id)
        {
            return await _unitOfWork.driverRepository.GetById(id);
        }

        public async Task InsertDriverAsync(Driver driver)
        {
            await _unitOfWork.driverRepository.Add(driver);
        }

        public async Task UpdateDriverAsync(Driver driver)
        {
            await _unitOfWork.driverRepository.Update(driver);
        }
    }
}
