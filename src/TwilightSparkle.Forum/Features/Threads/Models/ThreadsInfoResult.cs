using System.Collections.Generic;
using System.Linq;

using TwilightSparkle.Forum.Foundation.ThreadsService;

namespace TwilightSparkle.Forum.Features.Threads.Models
{
    public class ThreadsInfoResult
    {
        public IReadOnlyCollection<ThreadPreviewInfoResult> Threads { get; set; }

        public int StartIndex { get; set; }

        public int Size { get; set; }

        public int Amount { get; set; }


        public ThreadsInfoResult(ThreadsInfo threadsInfo)
        {
            Threads = threadsInfo.Threads.Select(t => new ThreadPreviewInfoResult(t)).ToList();
            StartIndex = threadsInfo.StartIndex;
            Size = threadsInfo.Size;
            Amount = threadsInfo.Amount;
        }
    }
}
