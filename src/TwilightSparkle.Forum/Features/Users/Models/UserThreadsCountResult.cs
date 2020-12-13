
using TwilightSparkle.Forum.Foundation.UsersInfo;

namespace TwilightSparkle.Forum.Features.Users.Models
{
    public class UserThreadsCountResult
    {
        public int Count { get; set; }


        public UserThreadsCountResult(UserThreadsCount threadsCount)
        {
            Count = threadsCount.Count;
        }
    }
}
