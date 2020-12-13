using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TwilightSparkle.Forum.Features.Images.Models;
using TwilightSparkle.Forum.Foundation.ImageStorage;

namespace TwilightSparkle.Forum.Features.Images
{
    [Route("api/images")]
    [ApiController]
    public class ImagesController : Controller
    {
        private readonly IImageStorageService _imageStorageService;

        private readonly ILogger<ImagesController> _logger;


        public ImagesController(IImageStorageService imageStorageService,
            ILogger<ImagesController> logger)
        {
            _imageStorageService = imageStorageService;

            _logger = logger;
        }


        [HttpGet("{id}")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            _logger.LogInformation($"Getting image with id - {id}");

            var loadImageResult = await _imageStorageService.LoadImageAsync(id);
            if (loadImageResult.IsSuccess)
            {
                _logger.LogInformation($"Getting image with id - {id} success");

                return File(loadImageResult.Value.File, loadImageResult.Value.FileMediaType);
            }

            var statusCodeResult = GetImageLoadErrorResult(loadImageResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Getting image with id - {id} failed; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Getting image with id - {id} failed; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [HttpPost("upload")]
        [Authorize]
        [ProducesResponseType(typeof(SavedImageDataResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SavedImageDataResponse>> UploadImage(IFormFile image)
        {
            if (image == null)
            {
                return BadRequest();
            }
            _logger.LogInformation($"Uploading new image with name - {image.Name}");

            var imageStream = image.OpenReadStream();
            var saveImageResult = await _imageStorageService.SaveImageAsync(image.FileName, imageStream);
            if (!saveImageResult.IsSuccess)
            {
                var statusCodeResult = GetImageSaveErrorResult(saveImageResult.ErrorType);
                _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                    ? $"Uploading new image with name - {image.Name} failed; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                    : $"Uploading new image with name - {image.Name} failed; Reason - {statusCodeResult.Value}");

                return statusCodeResult;
            }

            var imageUrl = GetImageUrl(saveImageResult.Value.ExternalId);
            var dataContract = new SavedImageDataResponse
            {
                Url = imageUrl,
                ExternalId = saveImageResult.Value.ExternalId
            };
            _logger.LogInformation($"Uploading new image with name - {image.Name} success");

            return dataContract;
        }



        private string GetImageUrl(string externalId)
        {
            return Url.Action(nameof(Get), "Images", new { id = externalId }, Request.Scheme);
        }

        private ObjectResult GetImageLoadErrorResult(LoadImageError error)
        {
            return error switch
            {
                LoadImageError.IncorrectExternalId => NotFound(new ErrorResponse("Incorrect image id")),
                LoadImageError.ImageNotExists => StatusCode(500, new ErrorResponse("Internal error. Image wasn't found")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private ObjectResult GetImageSaveErrorResult(SaveImageError errorType)
        {
            return errorType switch
            {
                SaveImageError.EmptyFilePath => BadRequest(new ErrorResponse("Empty filepath")),
                SaveImageError.TooBigImage => BadRequest(new ErrorResponse("Too big image")),
                SaveImageError.NotAllowedMediaType => BadRequest(new ErrorResponse("Not allowed media type")),
                SaveImageError.StorageError => StatusCode(500, new ErrorResponse("Internal error. Image can't be uploaded")),
                _ => throw new ArgumentOutOfRangeException(nameof(errorType), errorType, null),
            };
        }
    }
}
