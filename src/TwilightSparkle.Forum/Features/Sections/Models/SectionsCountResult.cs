using TwilightSparkle.Forum.Foundation.SectionsService;

namespace TwilightSparkle.Forum.Features.Sections.Models
{
    public class SectionsCountResult
    {
        public int Count { get; set; }


        public SectionsCountResult(SectionsCount sectionsCount)
        {
            Count = sectionsCount.Count;
        }
    }
}
