using AutoMapper;
using LibraryProxy.Domain.Entities;
using LibraryProxy.Infrastructure.ExternalApi.Models;

namespace LibraryProxy.Infrastructure.ExternalApi.Mappings;

public sealed class BookApiMappingProfile : Profile
{
    public BookApiMappingProfile()
    {
        CreateMap<BookApiModel, Book>();
    }
}
