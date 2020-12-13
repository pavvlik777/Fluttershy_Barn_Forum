using AutoMapper;

using IdentityServer4.Models;

namespace TwilightSparkle.Forum.IdentityServer.Mappers
{
    public static class ScopeMappers
    {
        static ScopeMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ScopeMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static ApiScope ToModel(this DomainModel.IdentityServer4.ApiScope entity)
        {
            return entity == null ? null : Mapper.Map<ApiScope>(entity);
        }

        public static DomainModel.IdentityServer4.ApiScope ToEntity(this ApiScope model)
        {
            return model == null ? null : Mapper.Map<DomainModel.IdentityServer4.ApiScope>(model);
        }
    }
}