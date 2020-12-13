using System;

using TwilightSparkle.Forum.Foundation.ThreadsService;

namespace TwilightSparkle.Forum.Features.Threads.Models
{
    public class ThreadCommentInfoResult
    {
        public string AuthorNickname { get; set; }

        public DateTime CommentTimeUtc { get; set; }

        public string Content { get; set; }


        public ThreadCommentInfoResult(ThreadCommentInfo threadCommentInfo)
        {
            AuthorNickname = threadCommentInfo.AuthorNickname;
            CommentTimeUtc = threadCommentInfo.CommentTimeUtc;
            Content = threadCommentInfo.Content;
        }
    }
}
