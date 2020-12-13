using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using IdentityServer4.Models;
using IdentityServer4.Services;

using Microsoft.Extensions.Logging;

using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Repository.Interfaces;

namespace TwilightSparkle.Forum.IdentityServer
{
    public class CustomProfileService : IProfileService
    {
        private readonly IRepository<User> _userRepository;

        private readonly ILogger<CustomProfileService> _logger;


        public CustomProfileService(IRepository<User> userRepository, ILogger<CustomProfileService> logger)
        {
            _userRepository = userRepository;

            _logger = logger;
        }


        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(context.Subject.Identity.Name))
                {
                    var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

                    if (!string.IsNullOrWhiteSpace(userId?.Value))
                    {
                        var user = await _userRepository.FirstOrDefaultAsync(u => u.Id.ToString() == userId.Value);

                        if (user != null)
                        {
                            var claims = GetUserClaims(user);

                            context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                        }
                    }
                }
                else
                {
                    var user = await _userRepository.FirstOrDefaultAsync(u => u.Username == context.Subject.Identity.Name);

                    if (user != null)
                    {
                        var claims = GetUserClaims(user);

                        context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get profile data");
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "user_id");

                if (!string.IsNullOrWhiteSpace(userId?.Value))
                {
                    var targerUserId = int.Parse(userId.Value);
                    var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == targerUserId);

                    context.IsActive = user != null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to determine is profile active");
            }
        }


        private static IEnumerable<Claim> GetUserClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("user_id", user.Id.ToString() ?? ""),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username)
            };

            return claims;
        }
    }
}
