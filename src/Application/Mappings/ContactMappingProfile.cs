using AutoMapper;
using ContactAgenda.Application.DTOs;
using ContactAgenda.Domain.Entities;

namespace ContactAgenda.Application.Mappings;

/// <summary>
/// AutoMapper profile for Contact mappings
/// </summary>
public class ContactMappingProfile : Profile
{
    public ContactMappingProfile()
    {
        CreateMap<Contact, ContactDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.Value));
    }
}
