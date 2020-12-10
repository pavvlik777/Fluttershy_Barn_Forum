using TwilightSparkle.Forum.Features.Authentication.Models;
using TwilightSparkle.Forum.Foundation.Authentication;

namespace TwilightSparkle.Forum.Features
{
    public static class MappingExtensions
    {
        public static SignUpDto Map(this SignUpRequest request) =>
            new SignUpDto(request.Username, request.Password, request.PasswordConfirmation, request.Email);
    }
}
