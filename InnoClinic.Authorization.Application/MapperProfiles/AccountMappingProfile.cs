using AutoMapper;
using InnoClinic.Authorization.Core.Models.AccountModels;

namespace InnoClinic.Authorization.Application.MapperProfiles
{
    /// <summary>
    /// Configures the mapping between AccountDto and AccountEntity types using AutoMapper.
    /// </summary>
    public class AccountMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountMappingProfile"/> class.
        /// </summary>
        public AccountMappingProfile()
        {
            CreateMap<AccountDto, AccountEntity>();
            CreateMap<AccountEntity, AccountDto>();
        }
    }
}