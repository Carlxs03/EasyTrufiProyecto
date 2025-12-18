using EasyTrufi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Core.Interfaces
{
    public interface INfcCardRepository
    {
        Task<IEnumerable<NfcCard>> GetAllCardsAsync();
        Task<NfcCard> GetCardByIdAsync(long id);
        Task InsertCardAsync(NfcCard card);

        Task UpdateCardAsync(NfcCard card);

        Task DeleteCardAsync(long id);

        Task<bool> HasActiveCardAsync(long userId);

        Task <bool> CardExistsAsync(string cardUID);

    }
}
