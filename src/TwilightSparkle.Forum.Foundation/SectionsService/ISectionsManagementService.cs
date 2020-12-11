using System.Threading.Tasks;

using TwilightSparkle.Common.Services;

namespace TwilightSparkle.Forum.Foundation.SectionsService
{
    public interface ISectionsManagementService
    {
        Task<ServiceResult<SectionsInfo, GetSectionsError>> GetSections(int startIndex, int size);

        Task<ServiceResult<SectionsCount, GetSectionsCountError>> GetSectionsCount();

        Task<ServiceResult<SectionThreadsInfo, GetSectionThreadsInfoError>> GetSectionThreads(string sectionName, int startIndex, int size);

        Task<ServiceResult<SectionThreadsCount, GetSectionThreadsCountError>> GetSectionThreadsCount(string sectionName);
    }
}
