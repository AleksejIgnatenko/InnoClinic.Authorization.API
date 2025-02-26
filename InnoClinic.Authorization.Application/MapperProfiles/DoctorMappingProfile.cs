using AutoMapper;
using InnoClinic.Authorization.Core.Models.DoctorModels;

namespace InnoClinic.Authorization.Application.MapperProfiles
{
    public class DoctorMappingProfile : Profile
    {
        public DoctorMappingProfile()
        {
            CreateMap<DoctorEntity, DoctorDto>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Account.Id));

            CreateMap<DoctorDto, DoctorEntity>()
                .ForMember(dest => dest.Account, opt => opt.Ignore());
        }
    }
}
