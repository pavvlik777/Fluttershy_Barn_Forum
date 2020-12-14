using System;

namespace TwilightSparkle.Forum.Foundation.UsersInfo
{
    public class UserThreadInfo
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Section { get; set; }

        public DateTime CreationDateTimeUtc { get; set; }
    }
}
