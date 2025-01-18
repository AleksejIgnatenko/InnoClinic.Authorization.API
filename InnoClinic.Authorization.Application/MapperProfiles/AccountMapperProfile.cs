using AutoMapper;
using InnoClinic.Authorization.API.Contracts;
using InnoClinic.Authorization.Core.Models;

namespace InnoClinic.Authorization.Application.MapperProfiles
{
    public class AccountMapperProfile : Profile
    {
        public AccountMapperProfile()
        {
            CreateMap<AccountRequest, AccountModel>();
        }
    }
}
