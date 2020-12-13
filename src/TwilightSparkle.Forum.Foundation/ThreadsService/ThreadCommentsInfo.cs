using System.Collections.Generic;

namespace TwilightSparkle.Forum.Foundation.ThreadsService
{
    public class ThreadCommentsInfo
    {
        public IReadOnlyCollection<ThreadCommentInfo> Comments { get; set; }

        public int StartIndex { get; set; }

        public int Size { get; set; }

        public int Amount { get; set; }
    }
}
