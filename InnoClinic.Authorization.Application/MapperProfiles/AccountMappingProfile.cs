using AutoMapper;
using InnoClinic.Authorization.Core.Models.AccountModels;

namespace InnoClinic.Authorization.Application.MapperProfiles
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<AccountDto, AccountEntity>();
            CreateMap<AccountEntity, AccountDto>();
        }
    }
}
