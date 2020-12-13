namespace TwilightSparkle.Forum.Foundation.ImageStorage
{
    public interface IFirebaseImageStorageConfiguration
    {
        string StorageBucket { get; }

        string ImagesDirectory { get; }
    }
}
