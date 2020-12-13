using AutoMapper;

using IdentityServer4.Models;

namespace TwilightSparkle.Forum.IdentityServer.Mappers
{
    public static class ApiResourceMappers
    {
        static ApiResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static ApiResource ToModel(this DomainModel.IdentityServer4.ApiResource entity)
        {
            return entity == null ? null : Mapper.Map<ApiResource>(entity);
        }

        public static DomainModel.IdentityServer4.ApiResource ToEntity(this ApiResource model)
        {
            return model == null ? null : Mapper.Map<DomainModel.IdentityServer4.ApiResource>(model);
        }
    }
}