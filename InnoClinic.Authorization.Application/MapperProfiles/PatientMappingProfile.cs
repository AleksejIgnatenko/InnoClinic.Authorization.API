using AutoMapper;
using InnoClinic.Appointments.Core.Models.PatientModels;
using InnoClinic.Authorization.Core.Models.AccountModels;

namespace InnoClinic.Authorization.Application.MapperProfiles
{
    public class PatientMappingProfile : Profile
    {
        public PatientMappingProfile()
        {
            CreateMap<PatientDto, PatientEntity>()
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => new AccountEntity { Id = src.AccountId }));
        }
    }
}
