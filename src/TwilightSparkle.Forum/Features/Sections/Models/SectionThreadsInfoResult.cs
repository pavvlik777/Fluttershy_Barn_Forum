using System.Collections.Generic;

using TwilightSparkle.Forum.Foundation.SectionsService;

namespace TwilightSparkle.Forum.Features.Sections.Models
{
    public class SectionThreadsInfoResult
    {
        public IReadOnlyCollection<SectionThreadInfo> Threads { get; set; }

        public int StartIndex { get; set; }

        public int Size { get; set; }

        public int Amount { get; set; }


        public SectionThreadsInfoResult(SectionThreadsInfo sectionThreads)
        {
            Threads = sectionThreads.Threads;
            StartIndex = sectionThreads.StartIndex;
            Size = sectionThreads.Size;
            Amount = sectionThreads.Amount;
        }
    }
}
