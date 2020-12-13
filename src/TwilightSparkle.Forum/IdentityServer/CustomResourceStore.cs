using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IdentityServer4.Models;
using IdentityServer4.Stores;

using Microsoft.Extensions.Configuration;

using TwilightSparkle.Forum.IdentityServer.Mappers;
using TwilightSparkle.Repository.Interfaces;

namespace TwilightSparkle.Forum.IdentityServer
{
    public class CustomResourceStore : IResourceStore
    {
        private readonly IRepository<DomainModel.IdentityServer4.ApiResource> _apiResourceRepository;
        private readonly IRepository<DomainModel.IdentityServer4.ApiScope> _apiScopeRepository;
        private readonly IRepository<DomainModel.IdentityServer4.IdentityResource> _identityResourceRepository;


        public CustomResourceStore(IRepository<DomainModel.IdentityServer4.ApiResource> apiResourceRepository,
            IRepository<DomainModel.IdentityServer4.ApiScope> apiScopeRepository,
            IRepository<DomainModel.IdentityServer4.IdentityResource> identityResourceRepository)
        {
            _apiResourceRepository = apiResourceRepository;
            _apiScopeRepository = apiScopeRepository;
            _identityResourceRepository = identityResourceRepository;
        }


        public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            var list = await _apiResourceRepository.WhereAsync(a => apiResourceNames.Contains(a.Name),
                x => x.Secrets,
                x => x.Scopes,
                x => x.UserClaims,
                x => x.Properties);

            return list.Select(r => r.ToModel()).ToList();
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var list = await _apiResourceRepository.WhereAsync(a => a.Scopes.Any(s => scopeNames.Contains(s.Scope)),
                x => x.Secrets,
                x => x.Scopes,
                x => x.UserClaims,
                x => x.Properties);

            return list.Select(r => r.ToModel()).ToList();
        }

        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            var list = await _apiScopeRepository.WhereAsync(s => scopeNames.Contains(s.Name),
                x => x.UserClaims,
                x => x.Properties);

            return list.Select(r => r.ToModel()).ToList();
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var list = await _identityResourceRepository.WhereAsync(e => scopeNames.Contains(e.Name),
                x => x.UserClaims,
                x => x.Properties);

            return list.Select(r => r.ToModel()).ToList();
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var result = new Resources(GetAllIdentityResources(), GetAllApiResources(), GetAllApiScopes());

            return Task.FromResult(result);
        }


        private IEnumerable<ApiResource> GetAllApiResources()
        {
            return _apiResourceRepository.All(null,
                x => x.Secrets,
                x => x.Scopes,
                x => x.UserClaims,
                x => x.Properties).Select(r => r.ToModel()).ToList();
        }

        private IEnumerable<IdentityResource> GetAllIdentityResources()
        {
            return _identityResourceRepository.All(null,
                x => x.UserClaims,
                x => x.Properties).Select(r => r.ToModel()).ToList();
        }

        private IEnumerable<ApiScope> GetAllApiScopes()
        {
            return _apiScopeRepository.All(null,
                x => x.UserClaims,
                x => x.Properties).Select(r => r.ToModel()).ToList();
        }
    }
}
