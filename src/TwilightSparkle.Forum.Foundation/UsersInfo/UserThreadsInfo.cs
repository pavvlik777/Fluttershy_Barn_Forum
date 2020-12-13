using System.Collections.Generic;

namespace TwilightSparkle.Forum.Foundation.UsersInfo
{
    public class UserThreadsInfo
    {
        public IReadOnlyCollection<UserThreadInfo> UserThreads { get; set; }

        public int StartIndex { get; set; }

        public int Size { get; set; }

        public int Amount { get; set; }
    }
}
