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
            INfcCardRepository NfcCardRepository,
            IUnitOfWork unitOfWork
            )
        {
            _NfcCardRepository = NfcCardRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<NfcCard>> GetAllCardsAsync()
        {
            //return await _unitOfWork.nfcCardRepository.GetAll();
            return await _NfcCardRepository.GetAllCardsAsync();
        }

        public async Task<NfcCard> GetCardByIdAsync(long id)
        {
            //return await _unitOfWork.nfcCardRepository.GetById(id);
            return await _NfcCardRepository.GetCardByIdAsync(id);
        }
        public async Task InsertCardAsync(NfcCard card)
        {
            //await _unitOfWork.nfcCardRepository.Add(card);
            await _NfcCardRepository.InsertCardAsync(card);
        }

        public async Task UpdateCardAsync(NfcCard card)
        {
            //await _unitOfWork.nfcCardRepository.Update(card);
            await _NfcCardRepository.UpdateCardAsync(card);
        }


        public async Task DeleteCardAsync(long id)
        {
            //await _unitOfWork.nfcCardRepository.Delete(id);
            await _NfcCardRepository.DeleteCardAsync(id);
        }

        public async Task<bool> HasActiveCardAsync(long userId)
        {
            return await _NfcCardRepository.HasActiveCardAsync(userId);
        }

        public async Task<bool> CardExistsAsync(string cardUID)
        {
            return await _NfcCardRepository.CardExistsAsync(cardUID);
        }

        public async Task AddTopupAsync(long nfcCardId, Topup topup)
        {


            var card = await _NfcCardRepository.GetCardByIdAsync(nfcCardId);
            if (card == null)
                throw new ArgumentException($"NfcCard con id {nfcCardId} no encontrada.");

            // Inicializar colección si hace falta
            card.Topups ??= new List<Topup>();

            // Asegurar la relación y añadir
            topup.NfcCardId = nfcCardId;
            card.Topups.Add(topup);

            // Persistir usando el repositorio de NfcCard (EF tracking debería insertar el Topup)
            await _NfcCardRepository.UpdateCardAsync(card);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
