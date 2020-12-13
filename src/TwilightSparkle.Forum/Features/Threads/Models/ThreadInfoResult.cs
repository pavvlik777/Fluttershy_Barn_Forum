using System;

using TwilightSparkle.Forum.Foundation.ThreadsService;

namespace TwilightSparkle.Forum.Features.Threads.Models
{
    public class ThreadInfoResult
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string SectionName { get; set; }

        public string AuthorUsername { get; set; }

        public DateTime CreationDateTimeUtc { get; set; }

        public int LikesDislikes { get; set; }


        public ThreadInfoResult(ThreadInfo threadInfo)
        {
            Id = threadInfo.Id;
            Title = threadInfo.Title;
            Content = threadInfo.Content;
            SectionName = threadInfo.SectionName;
            AuthorUsername = threadInfo.AuthorUsername;
            CreationDateTimeUtc = threadInfo.CreationDateTimeUtc;
            LikesDislikes = threadInfo.LikesDislikes;
        }
    }
}
