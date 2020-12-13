using System;

namespace TwilightSparkle.Forum.Foundation.ThreadsService
{
    public class ThreadInfo
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string SectionName { get; set; }

        public string AuthorUsername { get; set; }

        public DateTime CreationDateTimeUtc { get; set; }

        public int LikesDislikes { get; set; }
    }
}
