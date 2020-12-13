using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using IdentityServer4.Models;
using IdentityServer4.Validation;

using TwilightSparkle.Common.Hasher;
using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Repository.Interfaces;

namespace TwilightSparkle.Forum.IdentityServer
{
    public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IRepository<User> _userRepository;
        private readonly IHasher _hasher;


        public CustomResourceOwnerPasswordValidator(IRepository<User> userRepository, IHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }


        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var user = await _userRepository.SingleOrDefaultAsync(u => u.Username == context.UserName || u.Email == context.UserName);
                if (user == null)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "User does not exist");

                    return;
                }

                var passwordHash = _hasher.GetHash(context.Password);
                if (user.PasswordHash == passwordHash)
                {
                    context.Result = new GrantValidationResult(
                        subject: user.Id.ToString(),
                        authenticationMethod: "custom",
                        claims: GetUserClaims(user));

                    return;
                }

                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid password");
            }
            catch (Exception)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username/email or password");
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
