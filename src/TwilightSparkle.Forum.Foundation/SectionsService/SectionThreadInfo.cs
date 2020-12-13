using System;

namespace TwilightSparkle.Forum.Foundation.SectionsService
{
    public class SectionThreadInfo
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AuthorUsername { get; set; }

        public DateTime CreationDateTimeUtc { get; set; }
    }
}
