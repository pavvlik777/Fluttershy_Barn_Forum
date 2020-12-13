using TwilightSparkle.Forum.Foundation.ThreadsService;

namespace TwilightSparkle.Forum.Features.Threads.Models
{
    public class ThreadsCountResult
    {
        public int Count { get; set; }


        public ThreadsCountResult(ThreadsCount threadsCount)
        {
            Count = threadsCount.Count;
        }
    }
}
