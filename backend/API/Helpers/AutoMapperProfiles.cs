using API.Dtos;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{                                     // automapper library 3 party library.
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser , MemberDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt
            .MapFrom(src => src.Photos
            .FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Age , opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<Photo , PhotoDto>();


        }
    }
}