using System.Collections.Generic;

using TwilightSparkle.Forum.Foundation.UsersInfo;

namespace TwilightSparkle.Forum.Features.Users.Models
{
    public class UserThreadsInfoResult
    {
        public IReadOnlyCollection<UserThreadInfo> ThreadsInfo { get; set; }

        public int StartIndex { get; set; }

        public int Size { get; set; }

        public int Amount { get; set; }


        public UserThreadsInfoResult(UserThreadsInfo threadsInfo)
        {
            ThreadsInfo = threadsInfo.UserThreads;
            StartIndex = threadsInfo.StartIndex;
            Size = threadsInfo.Size;
            Amount = threadsInfo.Amount;
        }
    }
}
