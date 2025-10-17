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
        public NfcCardService(
            INfcCardRepository NfcCardRepository
            )
        {
            _NfcCardRepository = NfcCardRepository;
        }


        public async Task<IEnumerable<NfcCard>> GetAllCardsAsync()
        {
            return await _NfcCardRepository.GetAllCardsAsync();
        }

        public async Task<NfcCard> GetCardByIdAsync(int id)
        {
            return await _NfcCardRepository.GetCardByIdAsync(id);
        }
        public async Task InsertCardAsync(NfcCard card)
        {
            await _NfcCardRepository.InsertCardAsync(card);
        }

        public async Task UpdateCardAsync(NfcCard card)
        {
            await _NfcCardRepository.UpdateCardAsync(card);
        }


        public async Task DeleteCardAsync(int id)
        {
            await _NfcCardRepository.DeleteCardAsync(id);
        }

    }
}
