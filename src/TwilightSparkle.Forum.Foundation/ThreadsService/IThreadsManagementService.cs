using System.Threading.Tasks;

using TwilightSparkle.Common.Services;

namespace TwilightSparkle.Forum.Foundation.ThreadsService
{
    public interface IThreadsManagementService
    {
        Task<ServiceResult<ThreadsInfo, GetThreadsInfoError>> GetPopularThreads(int startIndex, int size);

        Task<ServiceResult<ThreadsCount, GetThreadsCountError>> GetPopularThreadsCount();


        Task<ServiceResult<ThreadInfo, GetThreadInfoError>> GetThreadInfo(int threadId);

        Task<ServiceResult<ThreadInfo, CreateThreadError>> CreateThread(CreateThreadDto request);

        Task<ServiceResult<DeleteThreadError>> DeleteThread(int threadId, string username);


        Task<ServiceResult<ThreadInfo, LikeDislikeThreadError>> LikeDislikeThread(int threadId, string username, bool isLike);


        Task<ServiceResult<ThreadCommentsInfo, GetThreadCommentsError>> GetThreadCommentsInfo(int threadId, int startIndex, int size);

        Task<ServiceResult<ThreadCommentsCount, GetThreadCommentsCountError>> GetThreadCommentsCount(int threadId);

        Task<ServiceResult<ThreadCommentInfo, CommentThreadError>> CommentThread(CommentThreadDto request);
    }
}
