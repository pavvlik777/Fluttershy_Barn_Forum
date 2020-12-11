using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TwilightSparkle.Forum.Features.Users.Models;
using TwilightSparkle.Forum.Foundation.UsersInfo;

namespace TwilightSparkle.Forum.Features.Users
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserInfoService _userInfoService;

        private readonly ILogger<UsersController> _logger;


        public UsersController(IUserInfoService userInfoService,
            ILogger<UsersController> logger)
        {
            _userInfoService = userInfoService;

            _logger = logger;
        }


        [Authorize]
        [HttpGet("current/data")]
        public async Task<IActionResult> GetCurrent()
        {
            _logger.LogInformation($"Getting info for user with username - {User.Identity.Name}");

            var userInfoResult = await _userInfoService.GetUserInfoByUsername(User.Identity.Name);
            if (userInfoResult.IsSuccess)
            {
                _logger.LogInformation($"Successful info retrieval for user with username - {User.Identity.Name}");

                return Ok(userInfoResult.Value);
            }

            var statusCodeResult = GetErrorResult(userInfoResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed info retrieval for user with username - {User.Identity.Name}; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed info retrieval for user with username - {User.Identity.Name}; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [HttpPatch("current/profile-image")]
        public async Task<IActionResult> UpdateCurrentProfileImage([FromBody, Required] UpdateCurrentProfileImageRequest request)
        {
            _logger.LogInformation($"Updating profile image for user with username - {User.Identity.Name}");

            var userInfoResult = await _userInfoService.UpdateProfileImage(User.Identity.Name, request.ImageExternalId);
            if (userInfoResult.IsSuccess)
            {
                _logger.LogInformation($"Updated profile image for user with username - {User.Identity.Name}");

                return Ok(userInfoResult.Value);
            }

            var statusCodeResult = GetErrorResult(userInfoResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed to update profile image for user with username - {User.Identity.Name}; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed to update profile image for user with username - {User.Identity.Name}; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }


        private ObjectResult GetErrorResult(GetUserInfoError error)
        {
            return error switch
            {
                GetUserInfoError.NotFound => NotFound(new ErrorResponse("User not found")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private ObjectResult GetErrorResult(UpdateProfileImageError error)
        {
            return error switch
            {
                UpdateProfileImageError.UserNotFound => NotFound(new ErrorResponse("User not found")),
                UpdateProfileImageError.ImageNotFound => NotFound(new ErrorResponse("Image not found")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }
    }
}
