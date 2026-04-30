namespace LibraryProxy.Application.Mappings;

public class AuthorMappingProfile : Profile
{
    public AuthorMappingProfile()
    {
        CreateMap<Author, AuthorDto>();
        CreateMap<AuthorDto, Author>();
        CreateMap<CreateAuthorDto, Author>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateAuthorDto, Author>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
