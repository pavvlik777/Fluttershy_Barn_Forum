using System.Collections.Generic;
using System.Linq;

using TwilightSparkle.Forum.Foundation.ThreadsService;

namespace TwilightSparkle.Forum.Features.Threads.Models
{
    public class ThreadCommentsInfoResult
    {
        public IReadOnlyCollection<ThreadCommentInfoResult> Comments { get; set; }

        public int StartIndex { get; set; }

        public int Size { get; set; }

        public int Amount { get; set; }


        public ThreadCommentsInfoResult(ThreadCommentsInfo commentsInfo)
        {
            Comments = commentsInfo.Comments.Select(c => new ThreadCommentInfoResult(c)).ToList();
            StartIndex = commentsInfo.StartIndex;
            Size = commentsInfo.Size;
            Amount = commentsInfo.Amount;
        }
    }
}
