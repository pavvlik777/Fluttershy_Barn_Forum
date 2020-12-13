using System.Threading.Tasks;

using IdentityServer4.Models;
using IdentityServer4.Stores;

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
            var client = await _repository.SingleOrDefaultAsync(c => c.ClientId == clientId);
            var result = client.ToModel();

            return result;
        }
    }
}
