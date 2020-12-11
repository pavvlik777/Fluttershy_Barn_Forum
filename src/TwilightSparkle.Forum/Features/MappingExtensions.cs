using TwilightSparkle.Forum.Features.Authentication.Models;
using TwilightSparkle.Forum.Features.Users.Models;
using TwilightSparkle.Forum.Foundation.Authentication;
using TwilightSparkle.Forum.Foundation.UsersInfo;

namespace TwilightSparkle.Forum.Features
{
    public static class MappingExtensions
    {
        public static SignUpDto Map(this SignUpRequest request) =>
            new SignUpDto(request.Username, request.Password, request.PasswordConfirmation, request.Email);

        public static GetUserThreadsInfoDto Map(this UserThreadsInfoRequest request, string username) =>
            new GetUserThreadsInfoDto
            {
                Size = request.Size.Value,
                StartIndex = request.StartIndex.Value,
                Username = username
            };
    }
}
