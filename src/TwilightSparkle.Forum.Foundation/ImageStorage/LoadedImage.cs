using System.IO;

namespace TwilightSparkle.Forum.Foundation.ImageStorage
{
    public class LoadedImage
    {
        public byte[] File { get; }

        public string FileMediaType { get; }


        public LoadedImage(byte[] file, string fileMediaType)
        {
            File = file;
            FileMediaType = fileMediaType;
        }
    }
}
