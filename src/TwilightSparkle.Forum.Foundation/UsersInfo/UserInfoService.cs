using System.Linq;
using System.Threading.Tasks;

using TwilightSparkle.Common.Services;
using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Repository.Interfaces;

namespace TwilightSparkle.Forum.Foundation.UsersInfo
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IForumUnitOfWork _unitOfWork;

        private const int MaxGetQuesrySize = 100;


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

        public async Task<ServiceResult<UserThreadsCount, GetUserThreadsCountError>> GetUserThreadsCount(string username)
        {
            var userRepository = _unitOfWork.UserRepository;
            var currentUser = await userRepository.FirstOrDefaultAsync(u => u.Username == username);
            if (currentUser == null)
            {
                return ServiceResult<UserThreadsCount, GetUserThreadsCountError>.CreateFailed(GetUserThreadsCountError.UserNotFound);
            }

            var threadsRepository = _unitOfWork.GetRepository<Thread>();
            var threads = await threadsRepository.WhereAsync(t => t.Author.Username == username, t => t.Author, t => t.Section);
            var count = threads.Count();
            var result = new UserThreadsCount
            {
                Count = count
            };

            return ServiceResult<UserThreadsCount, GetUserThreadsCountError>.CreateSuccess(result);
        }

        public async Task<ServiceResult<UserThreadsInfo, GetUserThreadsInfoError>> GetUserThreadsInfo(GetUserThreadsInfoDto request)
        {
            var userRepository = _unitOfWork.UserRepository;
            var currentUser = await userRepository.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (currentUser == null)
            {
                return ServiceResult<UserThreadsInfo, GetUserThreadsInfoError>.CreateFailed(GetUserThreadsInfoError.NotFound);
            }

            if (request.StartIndex < 0 || request.Size > MaxGetQuesrySize)
            {
                return ServiceResult<UserThreadsInfo, GetUserThreadsInfoError>.CreateFailed(GetUserThreadsInfoError.InvalidPaginationArguments);
            }

            var threadsRepository = _unitOfWork.GetRepository<Thread>();
            var threads = threadsRepository.All(t => t.Author.Username == request.Username, t => t.Author, t => t.Section)
                .OrderByDescending(t => t.CreationDateTimeUtc).Skip(request.StartIndex).Take(request.Size).ToList();
            var result = new UserThreadsInfo
            {
                Amount = threads.Count,
                UserThreads = threads.Select(t => new UserThreadInfo
                {
                    Title = t.Title,
                    CreationDateTimeUtc = t.CreationDateTimeUtc,
                    Section = t.Section.Name
                }).ToList(),
                Size = request.Size,
                StartIndex = request.StartIndex
            };

            return ServiceResult<UserThreadsInfo, GetUserThreadsInfoError>.CreateSuccess(result);
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
