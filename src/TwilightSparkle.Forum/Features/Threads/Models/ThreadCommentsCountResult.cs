using TwilightSparkle.Forum.Foundation.ThreadsService;

namespace TwilightSparkle.Forum.Features.Threads.Models
{
    public class ThreadCommentsCountResult
    {
        public int Count { get; set; }


        public ThreadCommentsCountResult(ThreadCommentsCount commentsCount)
        {
            Count = commentsCount.Count;
        }
    }
}
