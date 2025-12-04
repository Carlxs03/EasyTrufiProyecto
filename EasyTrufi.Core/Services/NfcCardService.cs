using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Core.Services
{
    public class NfcCardService : INfcCardService
    {
        public readonly INfcCardRepository _NfcCardRepository;
        public readonly IUnitOfWork _unitOfWork;
        public NfcCardService(
            //INfcCardRepository NfcCardRepository,
            IUnitOfWork unitOfWork
            )
        {
            //_NfcCardRepository = NfcCardRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<NfcCard>> GetAllCardsAsync()
        {
            return await _unitOfWork.nfcCardRepository.GetAll();
        }

        public async Task<NfcCard> GetCardByIdAsync(long id)
        {
            return await _unitOfWork.nfcCardRepository.GetById(id);
        }
        public async Task InsertCardAsync(NfcCard card)
        {
            await _unitOfWork.nfcCardRepository.Add(card);
        }

        public async Task UpdateCardAsync(NfcCard card)
        {
            await _unitOfWork.nfcCardRepository.Update(card);
        }


        public async Task DeleteCardAsync(long id)
        {
            await _unitOfWork.nfcCardRepository.Delete(id);
        }

    }
}
