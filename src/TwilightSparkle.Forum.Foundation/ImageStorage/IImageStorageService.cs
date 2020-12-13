using System.IO;
using System.Threading.Tasks;

using TwilightSparkle.Common.Services;

namespace TwilightSparkle.Forum.Foundation.ImageStorage
{
    public interface IImageStorageService
    {
        Task<ServiceResult<SavedImage, SaveImageError>> SaveImageAsync(string filePath, Stream imageStream);

        Task<ServiceResult<LoadedImage, LoadImageError>> LoadImageAsync(string externalId);
    }
}
