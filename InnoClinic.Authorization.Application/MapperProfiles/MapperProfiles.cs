using AutoMapper;
using InnoClinic.Authorization.Core.Dto;
using InnoClinic.Authorization.Core.Models;

namespace InnoClinic.Authorization.Application.MapperProfiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<AccountModel, AccountDto>();
        }
    }
}