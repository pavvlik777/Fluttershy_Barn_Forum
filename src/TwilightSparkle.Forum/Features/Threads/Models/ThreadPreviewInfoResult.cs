using System;

using TwilightSparkle.Forum.Foundation.ThreadsService;

namespace TwilightSparkle.Forum.Features.Threads.Models
{
    public class ThreadPreviewInfoResult
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AuthorUsername { get; set; }

        public DateTime CreationDateTimeUtc { get; set; }

        public int LikesDislikes { get; set; }


        public ThreadPreviewInfoResult(ThreadInfo threadInfo)
        {
            Id = threadInfo.Id;
            Title = threadInfo.Title;
            AuthorUsername = threadInfo.AuthorUsername;
            CreationDateTimeUtc = threadInfo.CreationDateTimeUtc;
            LikesDislikes = threadInfo.LikesDislikes;
        }
    }
}
