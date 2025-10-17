using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Interfaces;
using EasyTrufi.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Infraestructure.Repositories
{
    public class NfcCardRepository : INfcCardRepository
    {

        private readonly EasyTrufiContext _context;

        public NfcCardRepository(EasyTrufiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NfcCard>> GetAllCardsAsync()
        {
            var cards = await _context.NfcCards.ToListAsync();
            return cards;
        }

        public async Task<NfcCard> GetCardByIdAsync(int id)
        {
            var card = await _context.NfcCards.FirstOrDefaultAsync(x=>x.Id == id);
            return card;
        }

        public async Task InsertCardAsync(NfcCard card)
        {
            _context.NfcCards.Add(card);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCardAsync(NfcCard card)
        {
            _context.NfcCards.Update(card);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCardAsync(int id)
        {
            NfcCard card = await GetCardByIdAsync(id);
            _context.NfcCards.Remove(card);
            await _context.SaveChangesAsync();
        }

        




    }
}
