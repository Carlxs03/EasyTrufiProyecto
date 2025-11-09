using EasyTrufi.Core.CustomEntities;
using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Enum;
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
    internal class PaymentService : IPaymentService
    {
        //public readonly IBaseRepository<User> _userRepository;
        //public readonly IUserRepository _userRepository;

        public readonly IUnitOfWork _unitOfWork;

        public PaymentService(
            //IBaseRepository<User> userRepository
            //IUserRepository userRepository

            IUnitOfWork unitOfWork

            )
        {
            //_userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _unitOfWork.paymentRepository.GetAll();
            //return await _userRepository.GetAll();
            //return await _userRepository.GetAllUserAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            return await _unitOfWork.paymentRepository.GetById(id);
            //return await _userRepository.GetById(id);
        }

        public async Task InsertPaymentAsync(Payment payment)
        {
            await _unitOfWork.paymentRepository.Add(payment);
            //await _userRepository.Add(user);
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            await _unitOfWork.paymentRepository.Update(payment);

            //await _userRepository.Update(user);
        }

        public async Task DeletePaymentAsync(int id)
        {
            await _unitOfWork.paymentRepository.Delete(id);

            //await _userRepository.Delete(id);
        }

        public async Task<ResponseData> GetAllPaymentsAsync(PaymentQueryFilter filters)
        {
            var payments = await _unitOfWork.paymentRepository.GetAll();

            if (filters.AmountCents != null)
            {
                payments = payments.Where(x => x.AmountCents == filters.AmountCents);
            }
            if (filters.UserId != null)
            {
                payments = payments.Where(x => x.UserId == filters.UserId);
            }
            if (filters.ValidatorId != null)
            {
                payments = payments.Where(x => x.ValidatorId == filters.ValidatorId);
            }
            if (filters.NfcCardId != null)
            {
                payments = payments.Where(x => x.NfcCardId == filters.NfcCardId);
            }


            var pagedPayments = PagedList<object>.Create(payments, filters.PageNumber, filters.PageSize);
            if (pagedPayments.Any())
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.information.ToString(), Description = "Registros de payments recuperados correctamente" } },
                    Pagination = pagedPayments,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.warning.ToString(), Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedPayments,
                    StatusCode = HttpStatusCode.OK
                };
            }

        }
    }
}
