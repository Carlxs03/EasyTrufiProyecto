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
    public class ValidatorService : IValidatorService
    {
        public readonly IUnitOfWork _unitOfWork;
        public ValidatorService(
            //INfcCardRepository NfcCardRepository,
            IUnitOfWork unitOfWork
            )
        {
            //_NfcCardRepository = NfcCardRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteValidatorAsync(long id)
        {
            await _unitOfWork.validatorRepository.Delete(id);
        }

        public async Task<IEnumerable<Validator>> GetAllValidatorsAsync()
        {
            return await _unitOfWork.validatorRepository.GetAll();
        }

        public async Task<ResponseData> GetAllValidatorsAsync(ValidatorsQueryFilters filters)
        {
            var validators = await _unitOfWork.validatorRepository.GetAll();

            if (filters.ValidatorCode != null)
            {
                validators = validators.Where(x => x.ValidatorCode == filters.ValidatorCode);
            }

            if (filters.LocationDescription != null)
            {
                validators = validators.Where(x => x.LocationDescription == filters.LocationDescription);
            }

            if (filters.VehicleId != null)
            {
                validators = validators.Where(x => x.VehicleId == filters.VehicleId);
            }

            if (filters.Active != null)
            {
                validators = validators.Where(x => x.Active == filters.Active);
            }

            var pagedValidators = PagedList<object>.Create(validators, filters.PageNumber, filters.PageSize);
            if (pagedValidators.Any())
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de users recuperados correctamente" } },
                    Pagination = pagedValidators,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedValidators,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<Validator> GetValidatorByIdAsync(long id)
        {
            return await _unitOfWork.validatorRepository.GetById(id);
        }

        public async Task InsertValidatorAsync(Validator valid)
        {
            await _unitOfWork.validatorRepository.Add(valid);
        }

        public async Task UpdateValidatorAsync(Validator valid)
        {
            await _unitOfWork.validatorRepository.Update(valid);
        }
    }
}
