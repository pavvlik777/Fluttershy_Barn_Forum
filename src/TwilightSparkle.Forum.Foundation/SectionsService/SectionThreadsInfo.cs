using System.Collections.Generic;

namespace TwilightSparkle.Forum.Foundation.SectionsService
{
    public class SectionThreadsInfo
    {
        public IReadOnlyCollection<SectionThreadInfo> Threads { get; set; }

        public int StartIndex { get; set; }

        public int Size { get; set; }

        public int Amount { get; set; }
    }
}
