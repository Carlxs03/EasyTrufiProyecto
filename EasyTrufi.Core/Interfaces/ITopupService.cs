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
    public interface ITopupService
    {
        Task<IEnumerable<Topup>> GetAllTopupsAsync();

        Task<ResponseData> GetAllTopupsAsync(TopupQueryFilter filters);

        Task<Topup> GetTopupByIdAsync(long id);

        Task InsertTopupAsync(Topup topup);

        Task UpdateTopupAsync(Topup topup);

        Task DeleteTopupAsync(long id);
    }
}
