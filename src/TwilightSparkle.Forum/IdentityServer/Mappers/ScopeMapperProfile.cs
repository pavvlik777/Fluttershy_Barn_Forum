using System.Collections.Generic;

using AutoMapper;

using IdentityServer4.Models;

namespace TwilightSparkle.Forum.IdentityServer.Mappers
{
    public class ScopeMapperProfile : Profile
    {
        public ScopeMapperProfile()
        {
            CreateMap<DomainModel.IdentityServer4.ApiScopeProperty, KeyValuePair<string, string>>()
                .ReverseMap();

            CreateMap<DomainModel.IdentityServer4.ApiScopeClaim, string>()
               .ConstructUsing(x => x.Type)
               .ReverseMap()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            CreateMap<DomainModel.IdentityServer4.ApiScope, ApiScope>(MemberList.Destination)
                .ConstructUsing(src => new ApiScope())
                .ForMember(x => x.Properties, opts => opts.MapFrom(x => x.Properties))
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(x => x.UserClaims))
                .ReverseMap();
        }
    }
}
