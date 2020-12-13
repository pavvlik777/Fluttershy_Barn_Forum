using AutoMapper;

using IdentityServer4.Models;

namespace TwilightSparkle.Forum.IdentityServer.Mappers
{
    public static class ClientMappers
    {
        static ClientMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ClientMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static Client ToModel(this DomainModel.IdentityServer4.Client entity)
        {
            return Mapper.Map<Client>(entity);
        }

        public static DomainModel.IdentityServer4.Client ToEntity(this Client model)
        {
            return Mapper.Map<DomainModel.IdentityServer4.Client>(model);
        }
    }
}