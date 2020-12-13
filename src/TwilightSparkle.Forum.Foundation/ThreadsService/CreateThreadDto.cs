namespace TwilightSparkle.Forum.Foundation.ThreadsService
{
    public class CreateThreadDto
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string SectionName { get; set; }

        public string AuthorUsername { get; set; }
    }
}
