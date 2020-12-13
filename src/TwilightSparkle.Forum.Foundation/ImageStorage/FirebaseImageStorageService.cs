using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using TwilightSparkle.Common.Services;
using TwilightSparkle.Firebase.Storage;
using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Repository.Interfaces;

namespace TwilightSparkle.Forum.Foundation.ImageStorage
{
    public class FirebaseImageStorageService : IImageStorageService
    {
        private readonly IForumUnitOfWork _unitOfWork;

        private readonly IFirebaseImageStorageConfiguration _firebaseImageStorageConfiguration;
        private readonly IImageStorageConfiguration _imageStorageConfiguration;


        public FirebaseImageStorageService(IForumUnitOfWork unitOfWork,
            IFirebaseImageStorageConfiguration firebaseImageStorageConfiguration,
            IImageStorageConfiguration imageStorageConfiguration)
        {
            _unitOfWork = unitOfWork;

            _firebaseImageStorageConfiguration = firebaseImageStorageConfiguration;
            _imageStorageConfiguration = imageStorageConfiguration;
        }


        public async Task<ServiceResult<SavedImage, SaveImageError>> SaveImageAsync(string filePath, Stream imageStream)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return ServiceResult<SavedImage, SaveImageError>.CreateFailed(SaveImageError.EmptyFilePath);
            }

            if (imageStream.Length > _imageStorageConfiguration.MaximumImageSize)
            {
                return ServiceResult<SavedImage, SaveImageError>.CreateFailed(SaveImageError.TooBigImage);
            }

            var imageExtension = Path.GetExtension(filePath);
            var mediaType = MimeMapping.MimeUtility.GetMimeMapping(filePath);
            var isMediaTypeAllowed = _imageStorageConfiguration.AllowedImageMediaTypes.Any(t => t == mediaType);
            if (!isMediaTypeAllowed)
            {
                return ServiceResult<SavedImage, SaveImageError>.CreateFailed(SaveImageError.NotAllowedMediaType);
            }
            var externalId = Guid.NewGuid().ToString();
            var uploadedImageName = $"{externalId}{imageExtension}";

            var firebaseStorage = new FirebaseStorage(_firebaseImageStorageConfiguration.StorageBucket)
                .Child(_firebaseImageStorageConfiguration.ImagesDirectory)
                .Child(uploadedImageName);

            string downloadUri;
            try
            {
                downloadUri = await firebaseStorage.PutAsync(imageStream);
            }
            catch (FirebaseStorageException)
            {
                return ServiceResult<SavedImage, SaveImageError>.CreateFailed(SaveImageError.StorageError);
            }

            var uploadedImage = new UploadedImage
            {
                FilePath = downloadUri,
                ExternalId = externalId,
                MediaType = mediaType
            };

            var imagesRepository = _unitOfWork.GetRepository<UploadedImage>();
            imagesRepository.Create(uploadedImage);
            await _unitOfWork.SaveAsync();

            return ServiceResult<SavedImage, SaveImageError>.CreateSuccess(new SavedImage(uploadedImage.ExternalId));
        }

        public async Task<ServiceResult<LoadedImage, LoadImageError>> LoadImageAsync(string externalId)
        {
            var imagesRepository = _unitOfWork.GetRepository<UploadedImage>();
            var imageDetails = await imagesRepository.SingleOrDefaultAsync(i => i.ExternalId == externalId);
            if (imageDetails == null)
            {
                return ServiceResult<LoadedImage, LoadImageError>.CreateFailed(LoadImageError.IncorrectExternalId);
            }

            var imageExtension = Path.GetExtension(imageDetails.FilePath);
            var imageName = $"{externalId}{imageExtension}";

            var firebaseStorage = new FirebaseStorage(_firebaseImageStorageConfiguration.StorageBucket)
                .Child(_firebaseImageStorageConfiguration.ImagesDirectory)
                .Child(imageName);

            try
            {
                var fileBytes = await firebaseStorage.GetAsync(imageDetails.FilePath);

                return ServiceResult<LoadedImage, LoadImageError>.CreateSuccess(new LoadedImage(fileBytes, imageDetails.MediaType));
            }
            catch (FirebaseStorageException)
            {
                return ServiceResult<LoadedImage, LoadImageError>.CreateFailed(LoadImageError.ImageNotExists);
            }
        }
    }
}
