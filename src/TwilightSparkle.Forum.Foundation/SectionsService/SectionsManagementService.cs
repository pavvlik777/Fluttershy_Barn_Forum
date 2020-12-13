using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using TwilightSparkle.Common.Services;
using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Repository.Interfaces;

namespace TwilightSparkle.Forum.Foundation.SectionsService
{
    public class SectionsManagementService : ISectionsManagementService
    {
        private readonly IForumUnitOfWork _unitOfWork;

        private const int MaxGetQuerySize = 100;


        public SectionsManagementService(IForumUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ServiceResult<SectionsInfo, GetSectionsError>> GetSections(int startIndex, int size)
        {
            if (startIndex < 0 || size > MaxGetQuerySize)
            {
                return ServiceResult<SectionsInfo, GetSectionsError>.CreateFailed(GetSectionsError.InvalidPaginationArguments);
            }

            var sectionRepository = _unitOfWork.GetRepository<Section>();
            var sections = await sectionRepository.All(null)
                .OrderBy(s => s.Name).Skip(startIndex).Take(size).ToListAsync();
            var result = new SectionsInfo
            {
                Amount = sections.Count,
                Sections = sections.Select(s => new SectionInfo
                {
                    Name = s.Name
                }).ToList(),
                Size = size,
                StartIndex = startIndex
            };

            return ServiceResult<SectionsInfo, GetSectionsError>.CreateSuccess(result);
        }

        public async Task<ServiceResult<SectionsCount, GetSectionsCountError>> GetSectionsCount()
        {
            var sectionRepository = _unitOfWork.GetRepository<Section>();
            var sections = sectionRepository.All(null);
            var count = await sections.CountAsync();
            var result = new SectionsCount
            {
                Count = count
            };

            return ServiceResult<SectionsCount, GetSectionsCountError>.CreateSuccess(result);
        }

        public async Task<ServiceResult<SectionThreadsInfo, GetSectionThreadsInfoError>> GetSectionThreads(string sectionName, int startIndex, int size)
        {
            var sectionRepository = _unitOfWork.GetRepository<Section>();
            var section = await sectionRepository.FirstOrDefaultAsync(s => s.Name == sectionName);
            if (section == null)
            {
                return ServiceResult<SectionThreadsInfo, GetSectionThreadsInfoError>.CreateFailed(GetSectionThreadsInfoError.InvalidSection);
            }

            if (startIndex < 0 || size > MaxGetQuerySize)
            {
                return ServiceResult<SectionThreadsInfo, GetSectionThreadsInfoError>.CreateFailed(GetSectionThreadsInfoError.InvalidPaginationArguments);
            }

            var threadRepository = _unitOfWork.GetRepository<Thread>();
            var threads = await threadRepository.All(t => t.SectionId == section.Id, t => t.Author)
                .OrderByDescending(t => t.CreationDateTimeUtc).Skip(startIndex).Take(size).ToListAsync();
            var result = new SectionThreadsInfo
            {
                Amount = threads.Count,
                Threads = threads.Select(t => new SectionThreadInfo
                {
                    Id = t.Id,
                    Title = t.Title,
                    CreationDateTimeUtc = t.CreationDateTimeUtc,
                    AuthorUsername = t.Author.Username
                }).ToList(),
                Size = size,
                StartIndex = startIndex
            };

            return ServiceResult<SectionThreadsInfo, GetSectionThreadsInfoError>.CreateSuccess(result);
        }

        public async Task<ServiceResult<SectionThreadsCount, GetSectionThreadsCountError>> GetSectionThreadsCount(string sectionName)
        {
            var sectionRepository = _unitOfWork.GetRepository<Section>();
            var section = await sectionRepository.FirstOrDefaultAsync(s => s.Name == sectionName);
            if (section == null)
            {
                return ServiceResult<SectionThreadsCount, GetSectionThreadsCountError>.CreateFailed(GetSectionThreadsCountError.InvalidSection);
            }

            var threadRepository = _unitOfWork.GetRepository<Thread>();
            var threads = threadRepository.All(t => t.SectionId == section.Id);
            var count = await threads.CountAsync();
            var result = new SectionThreadsCount
            {
                Count = count
            };

            return ServiceResult<SectionThreadsCount, GetSectionThreadsCountError>.CreateSuccess(result);
        }
    }
}
