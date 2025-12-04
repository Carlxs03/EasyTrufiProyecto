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
    public class TopupService : ITopupService
    {
        //public readonly IBaseRepository<User> _userRepository;
        //public readonly IUserRepository _userRepository;

        public readonly IUnitOfWork _unitOfWork;

        public TopupService(
            //IBaseRepository<User> userRepository
            //IUserRepository userRepository

            IUnitOfWork unitOfWork

            )
        {
            //_userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteTopupAsync(long id)
        {
            await _unitOfWork.topupRepository.Delete(id);
        }

        public async Task<IEnumerable<Topup>> GetAllTopupsAsync()
        {
            return await _unitOfWork.topupRepository.GetAll();
        }

        public async Task<ResponseData> GetAllTopupsAsync(TopupQueryFilter filters)
        {
            var topups = await _unitOfWork.topupRepository.GetAll();

            if (filters.AmountCents != null)
            {
                topups = topups.Where(x => x.AmountCents == filters.AmountCents);
            }

            if (filters.Reference != null)
            {
                topups = topups.Where(x => x.Reference == filters.Reference);
            }

            if (filters.UserId != null)
            {
                topups = topups.Where(x => x.UserId == filters.UserId);
            }

            if (filters.Status != null)
            {
                topups = topups.Where(x => x.Status == filters.Status);
            }

            if (filters.Method != null)
            {
                topups = topups.Where(x => x.Method == filters.Method);
            }

            if (filters.NfcCardId != null)
            {
                topups = topups.Where(x => x.NfcCardId == filters.NfcCardId);
            }

        
            var pagedTopups = PagedList<object>.Create(topups, filters.PageNumber, filters.PageSize);
            if (pagedTopups.Any())
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de users recuperados correctamente" } },
                    Pagination = pagedTopups,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedTopups,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<Topup> GetTopupByIdAsync(long id)
        {
            return await _unitOfWork.topupRepository.GetById(id);
        }

        public async Task InsertTopupAsync(Topup topup)
        {
            await _unitOfWork.topupRepository.Add(topup);
        }

        public async Task UpdateTopupAsync(Topup topup)
        {
            await _unitOfWork.topupRepository.Update(topup);
        }
    }
}
