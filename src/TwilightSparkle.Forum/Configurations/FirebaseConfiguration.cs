using TwilightSparkle.Forum.Foundation.ImageStorage;

namespace TwilightSparkle.Forum.Configurations
{
    public class FirebaseConfiguration : IFirebaseImageStorageConfiguration
    {
        public string StorageBucket { get; set; }

        public string ImagesDirectory { get; set; }
    }
}
