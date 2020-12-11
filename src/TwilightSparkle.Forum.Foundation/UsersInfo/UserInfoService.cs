using System.Threading.Tasks;

using TwilightSparkle.Common.Services;
using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Repository.Interfaces;

namespace TwilightSparkle.Forum.Foundation.UsersInfo
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IForumUnitOfWork _unitOfWork;


        public UserInfoService(IForumUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ServiceResult<UserInfo, GetUserInfoError>> GetUserInfoByUsername(string username)
        {
            var userRepository = _unitOfWork.UserRepository;
            var currentUser = await userRepository.FirstOrDefaultAsync(u => u.Username == username, u => u.ProfileImage);
            if (currentUser == null)
            {
                return ServiceResult<UserInfo, GetUserInfoError>.CreateFailed(GetUserInfoError.NotFound);
            }

            var userInfo = new UserInfo
            {
                Username = currentUser.Username,
                Email = currentUser.Email,
                ProfileImageExternalId = currentUser.ProfileImage?.ExternalId
            };

            return ServiceResult<UserInfo, GetUserInfoError>.CreateSuccess(userInfo);
        }

        public async Task<ServiceResult<UserInfo, UpdateProfileImageError>> UpdateProfileImage(string username, string imageExternalId)
        {
            var userRepository = _unitOfWork.UserRepository;
            var currentUser = await userRepository.FirstOrDefaultAsync(u => u.Username == username);
            if (currentUser == null)
            {
                return ServiceResult<UserInfo, UpdateProfileImageError>.CreateFailed(UpdateProfileImageError.UserNotFound);
            }

            var imagesRepository = _unitOfWork.GetRepository<UploadedImage>();
            var profileImage = await imagesRepository.FirstOrDefaultAsync(i => i.ExternalId == imageExternalId);
            if (profileImage == null)
            {
                return ServiceResult<UserInfo, UpdateProfileImageError>.CreateFailed(UpdateProfileImageError.ImageNotFound);
            }

            var userInfo = new UserInfo
            {
                Username = currentUser.Username,
                Email = currentUser.Email,
                ProfileImageExternalId = profileImage.ExternalId,
            };

            currentUser.ProfileImageId = profileImage.Id;
            userRepository.Update(currentUser);

            await _unitOfWork.SaveAsync();

            return ServiceResult<UserInfo, UpdateProfileImageError>.CreateSuccess(userInfo);
        }
    }
}
