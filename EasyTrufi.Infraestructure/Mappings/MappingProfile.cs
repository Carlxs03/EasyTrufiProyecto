using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using EasyTrufi.Core.Entities;
using EasyTrufi.Infraestructure.DTOs;

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


            CreateMap<NfcCard, NfcCardDTO>();
            CreateMap<NfcCardDTO, NfcCard>();


        }
    }
}
