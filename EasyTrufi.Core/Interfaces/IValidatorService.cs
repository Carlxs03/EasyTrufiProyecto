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
    public interface IValidatorService
    {
        Task<IEnumerable<Validator>> GetAllValidatorsAsync();

        Task<ResponseData> GetAllValidatorsAsync(ValidatorsQueryFilters filters);

        Task<Validator> GetValidatorByIdAsync(long id);

        Task InsertValidatorAsync(Validator valid);

        Task UpdateValidatorAsync(Validator valid);

        Task DeleteValidatorAsync(long id);
    }
}
