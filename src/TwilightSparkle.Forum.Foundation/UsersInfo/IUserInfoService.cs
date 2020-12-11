using System.Threading.Tasks;

using TwilightSparkle.Common.Services;

namespace TwilightSparkle.Forum.Foundation.UsersInfo
{
    public interface IUserInfoService
    {
        Task<ServiceResult<UserInfo, GetUserInfoError>> GetUserInfoByUsername(string username);

        Task<ServiceResult<UserInfo, UpdateProfileImageError>> UpdateProfileImage(string username, string imageExternalId);
    }
}
