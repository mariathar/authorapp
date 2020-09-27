using AuthorApp.ApiContracts.Request;
using AuthorApp.ApiContracts.Response;
using AuthorApp.DataAccessContracts.Models;
using AutoMapper;

namespace AuthorApp.Mapper
{
    public class AppMapper : Profile
    {
        public AppMapper()
        {
            CreateMap<AuthorDto, AuthorRecordResponse>();
            CreateMap<AuthorDto, AuthorResponse>();
            CreateMap<AuthorCreateRequest, AuthorCreateDto>();
            CreateMap<AuthorUpdateRequest, AuthorUpdateDto>();
        }
    }
}
