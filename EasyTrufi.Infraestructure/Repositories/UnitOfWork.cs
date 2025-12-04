using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Interfaces;
using EasyTrufi.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Infraestructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EasyTrufiContext _context;
        public readonly IUserRepository _userRepository;
        public readonly IBaseRepository<NfcCard> _nfcCardRepository;
        public readonly IBaseRepository<Driver> _driverRepository;
        public readonly IBaseRepository<Payment> _paymentRepository;
        public readonly IBaseRepository<Topup> _topupRepository;
        public readonly IBaseRepository<Validator> _validatorRepository;

        private readonly ISecurityRepository _securityRepository;


        public readonly IDapperContext _dapper;

        private IDbContextTransaction? _efTransaction;

        public UnitOfWork(EasyTrufiContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }



        public IUserRepository userRepository =>
            _userRepository ?? new UserRepository(_context, _dapper);

        public IBaseRepository<NfcCard> nfcCardRepository => 
            _nfcCardRepository ?? new BaseRepository<NfcCard>(_context);

        public IBaseRepository<Driver> driverRepository => 
            _driverRepository ?? new BaseRepository<Driver>(_context);

        public IBaseRepository<Payment> paymentRepository => 
            _paymentRepository ?? new BaseRepository<Payment>(_context);

        public IBaseRepository<Topup> topupRepository => 
            _topupRepository ?? new BaseRepository<Topup>(_context);

        public IBaseRepository<Validator> validatorRepository => 
            _validatorRepository ?? new BaseRepository<Validator>(_context);



        public ISecurityRepository SecurityRepository =>
            _securityRepository ?? new SecurityRepository(_context);


        public void Dispose()
        {
            if(_context != null)
            {
                
                _context.Dispose();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #region Transacciones
        public async Task BeginTransactionAsync()
        {
            if (_efTransaction == null)
            {
                _efTransaction = await _context.Database.BeginTransactionAsync();

                // registrar la conexión/tx en DapperContext
                var conn = _context.Database.GetDbConnection();
                var tx = _efTransaction.GetDbTransaction();
                _dapper.SetAmbientConnection(conn, tx);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_efTransaction != null)
                {
                    await _efTransaction.CommitAsync();
                    _efTransaction.Dispose();
                    _efTransaction = null;
                }
            }
            finally
            {
                _dapper.ClearAmbientConnection();
            }
        }

        public async Task RollbackAsync()
        {
            if (_efTransaction != null)
            {
                await _efTransaction.RollbackAsync();
                _efTransaction.Dispose();
                _efTransaction = null;
            }
            _dapper.ClearAmbientConnection();
        }

        public IDbConnection? GetDbConnection()
        {
            // Retornamos la conexión subyacente del DbContext
            return _context.Database.GetDbConnection();
        }

        public IDbTransaction? GetDbTransaction()
        {
            return _efTransaction?.GetDbTransaction();
        }
        #endregion

    }
}
