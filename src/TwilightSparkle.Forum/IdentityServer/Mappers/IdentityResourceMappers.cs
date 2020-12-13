using AutoMapper;

using IdentityServer4.Models;

namespace TwilightSparkle.Forum.IdentityServer.Mappers
{
    public static class IdentityResourceMappers
    {
        static IdentityResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static IdentityResource ToModel(this DomainModel.IdentityServer4.IdentityResource entity)
        {
            return entity == null ? null : Mapper.Map<IdentityResource>(entity);
        }

        public static DomainModel.IdentityServer4.IdentityResource ToEntity(this IdentityResource model)
        {
            return model == null ? null : Mapper.Map<DomainModel.IdentityServer4.IdentityResource>(model);
        }
    }
}