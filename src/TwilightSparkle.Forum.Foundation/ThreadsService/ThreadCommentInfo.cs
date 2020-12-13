using System;

namespace TwilightSparkle.Forum.Foundation.ThreadsService
{
    public class ThreadCommentInfo
    {
        public string AuthorNickname { get; set; }

        public DateTime CommentTimeUtc { get; set; }

        public string Content { get; set; }
    }
}
