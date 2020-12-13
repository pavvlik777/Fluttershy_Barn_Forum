using System.Threading.Tasks;

using TwilightSparkle.Common.Services;

namespace TwilightSparkle.Forum.Foundation.UsersInfo
{
    public interface IUserInfoService
    {
        Task<ServiceResult<UserInfo, GetUserInfoError>> GetUserInfoByUsername(string username);

        Task<ServiceResult<UserThreadsCount, GetUserThreadsCountError>> GetUserThreadsCount(string username);

        Task<ServiceResult<UserThreadsInfo, GetUserThreadsInfoError>> GetUserThreadsInfo(GetUserThreadsInfoDto request);

        Task<ServiceResult<UserInfo, UpdateProfileImageError>> UpdateProfileImage(string username, string imageExternalId);
    }
}
