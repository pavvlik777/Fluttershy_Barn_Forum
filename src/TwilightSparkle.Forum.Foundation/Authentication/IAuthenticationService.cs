using System.Threading.Tasks;

using TwilightSparkle.Common.Services;

namespace TwilightSparkle.Forum.Foundation.Authentication
{
    public interface IAuthenticationService
    {
        Task<ServiceResult<SignUpError>> SignUp(SignUpDto signUpDto);
    }
}