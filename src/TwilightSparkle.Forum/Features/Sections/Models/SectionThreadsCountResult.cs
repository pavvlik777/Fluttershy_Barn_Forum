using TwilightSparkle.Forum.Foundation.SectionsService;

namespace TwilightSparkle.Forum.Features.Sections.Models
{
    public class SectionThreadsCountResult
    {
        public int Count { get; set; }


        public SectionThreadsCountResult(SectionThreadsCount sectionThreadsCount)
        {
            Count = sectionThreadsCount.Count;
        }
    }
}
