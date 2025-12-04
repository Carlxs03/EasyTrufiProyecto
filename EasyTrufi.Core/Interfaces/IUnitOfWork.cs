using EasyTrufi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EasyTrufi.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository userRepository { get; }

        IBaseRepository<NfcCard> nfcCardRepository { get; }

        IBaseRepository<Driver> driverRepository { get; }

        IBaseRepository<Payment> paymentRepository { get; }

        IBaseRepository<Topup> topupRepository { get; }

        IBaseRepository<Validator> validatorRepository { get; }

        ISecurityRepository SecurityRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();






        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        // Nuevos miembros para Dapper
        IDbConnection? GetDbConnection();
        IDbTransaction? GetDbTransaction();

    }
}
