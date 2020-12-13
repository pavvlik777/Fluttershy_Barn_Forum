using System.Collections.Generic;

namespace TwilightSparkle.Forum.Foundation.ImageStorage
{
    public interface IImageStorageConfiguration
    {
        int MaximumImageSize { get; }

        IReadOnlyCollection<string> AllowedImageMediaTypes { get; }
    }
}