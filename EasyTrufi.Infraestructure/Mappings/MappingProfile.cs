using AutoMapper;
using EasyTrufi.Core.Entities;
using EasyTrufi.Infraestructure.Data;
using EasyTrufi.Infraestructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Infraestructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<User, UserDTO_GetAll>();
            CreateMap<UserDTO_GetAll, User>();

            CreateMap<User, UserResponseDTO>();
            CreateMap<UserResponseDTO, User>();


            CreateMap<NfcCard, NfcCardDTO>();
            CreateMap<NfcCardDTO, NfcCard>();

            CreateMap<NfcCard, NfcCardBalanceDTO>();
            CreateMap<NfcCardBalanceDTO, NfcCard>();

            CreateMap<Security, SecurityDTO>().ReverseMap();

            CreateMap<Validator, ValidatorDTO>();
            CreateMap<ValidatorDTO, Validator>();

            CreateMap<Topup, TopupDTO>();
            CreateMap<TopupDTO, Topup>();

            CreateMap<Driver, DriverDTO>();
            CreateMap<DriverDTO, Driver>();

            CreateMap<Payment, PaymentDTO>();
            CreateMap<PaymentDTO, Payment>();

        }
    }
}
