using System.Linq;
using System.Threading.Tasks;

using IdentityServer4.Models;
using IdentityServer4.Stores;

using Microsoft.EntityFrameworkCore;

using TwilightSparkle.Forum.IdentityServer.Mappers;
using TwilightSparkle.Repository.Interfaces;

namespace TwilightSparkle.Forum.IdentityServer
{
    public class CustomClientStore : IClientStore
    {
        private readonly IRepository<DomainModel.IdentityServer4.Client> _repository;


        public CustomClientStore(IRepository<DomainModel.IdentityServer4.Client> repository)
        {
            _repository = repository;
        }


        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var clientsQuery = _repository.All(null);

            await clientsQuery.Include(x => x.AllowedCorsOrigins).SelectMany(c => c.AllowedCorsOrigins).LoadAsync();
            await clientsQuery.Include(x => x.AllowedGrantTypes).SelectMany(c => c.AllowedGrantTypes).LoadAsync();
            await clientsQuery.Include(x => x.AllowedScopes).SelectMany(c => c.AllowedScopes).LoadAsync();
            await clientsQuery.Include(x => x.Claims).SelectMany(c => c.Claims).LoadAsync();
            await clientsQuery.Include(x => x.ClientSecrets).SelectMany(c => c.ClientSecrets).LoadAsync();
            await clientsQuery.Include(x => x.IdentityProviderRestrictions).SelectMany(c => c.IdentityProviderRestrictions).LoadAsync();
            await clientsQuery.Include(x => x.PostLogoutRedirectUris).SelectMany(c => c.PostLogoutRedirectUris).LoadAsync();
            await clientsQuery.Include(x => x.Properties).SelectMany(c => c.Properties).LoadAsync();
            await clientsQuery.Include(x => x.RedirectUris).SelectMany(c => c.RedirectUris).LoadAsync();

            var client = await _repository.SingleOrDefaultAsync(c => c.ClientId == clientId);
            var result = client.ToModel();

            return result;
        }
    }
}
