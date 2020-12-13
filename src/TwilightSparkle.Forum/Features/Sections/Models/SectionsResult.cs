using System.Collections.Generic;

using TwilightSparkle.Forum.Foundation.SectionsService;

namespace TwilightSparkle.Forum.Features.Sections.Models
{
    public class SectionsResult
    {
        public IReadOnlyCollection<SectionInfo> Sections { get; set; }

        public int StartIndex { get; set; }

        public int Size { get; set; }

        public int Amount { get; set; }


        public SectionsResult(SectionsInfo sections)
        {
            Sections = sections.Sections;
            StartIndex = sections.StartIndex;
            Size = sections.Size;
            Amount = sections.Amount;
        }
    }
}
