using System.Collections.Generic;

using AutoMapper;

using IdentityServer4.Models;

namespace TwilightSparkle.Forum.IdentityServer.Mappers
{
    public class ApiResourceMapperProfile : Profile
    {
        public ApiResourceMapperProfile()
        {
            CreateMap<DomainModel.IdentityServer4.ApiResourceProperty, KeyValuePair<string, string>>()
                .ReverseMap();

            CreateMap<DomainModel.IdentityServer4.ApiResource, ApiResource>(MemberList.Destination)
                .ConstructUsing(src => new ApiResource())
                .ForMember(x => x.ApiSecrets, opts => opts.MapFrom(x => x.Secrets))
                .ForMember(x => x.AllowedAccessTokenSigningAlgorithms, opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter, x => x.AllowedAccessTokenSigningAlgorithms))
                .ReverseMap()
                .ForMember(x => x.AllowedAccessTokenSigningAlgorithms, opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter, x => x.AllowedAccessTokenSigningAlgorithms));

            CreateMap<DomainModel.IdentityServer4.ApiResourceClaim, string>()
                .ConstructUsing(x => x.Type)
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            CreateMap<DomainModel.IdentityServer4.ApiResourceSecret, Secret>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();

            CreateMap<DomainModel.IdentityServer4.ApiResourceScope, string>()
                .ConstructUsing(x => x.Scope)
                .ReverseMap()
                .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src));
        }
    }
}
