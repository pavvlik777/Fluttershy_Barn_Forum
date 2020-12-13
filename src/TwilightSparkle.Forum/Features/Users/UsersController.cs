using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TwilightSparkle.Forum.Features.Users.Models;
using TwilightSparkle.Forum.Foundation.UsersInfo;

namespace TwilightSparkle.Forum.Features.Users
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
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


        [HttpGet("current/data")]
        [ProducesResponseType(typeof(UserInfo), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        [HttpGet("current/threads")]
        [ProducesResponseType(typeof(UserThreadsInfoResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCurrentThreads([FromQuery, Required] UserThreadsInfoRequest request)
        {
            _logger.LogInformation($"Getting threads info for user with username - {User.Identity.Name}");

            var usersInfoResult = await _userInfoService.GetUserThreadsInfo(request.Map(User.Identity.Name));
            if (usersInfoResult.IsSuccess)
            {
                _logger.LogInformation($"Successful threads info retrieval for user with username - {User.Identity.Name}");

                return Ok(new UserThreadsInfoResult(usersInfoResult.Value));
            }

            var statusCodeResult = GetErrorResult(usersInfoResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed threads info retrieval for user with username - {User.Identity.Name}; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed threads info retrieval for user with username - {User.Identity.Name}; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [HttpGet("current/threads/count")]
        [ProducesResponseType(typeof(UserThreadsCountResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCurrentThreadsCount()
        {
            _logger.LogInformation($"Getting threads count for user with username - {User.Identity.Name}");

            var threadsCountResult = await _userInfoService.GetUserThreadsCount(User.Identity.Name);
            if (threadsCountResult.IsSuccess)
            {
                _logger.LogInformation($"Successful threads count retrieval for user with username - {User.Identity.Name}");

                return Ok(new UserThreadsCountResult(threadsCountResult.Value));
            }

            var statusCodeResult = GetErrorResult(threadsCountResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed users threads retrieval for user with username - {User.Identity.Name}; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed users threads retrieval for user with username - {User.Identity.Name}; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [HttpPatch("current/profile-image")]
        [ProducesResponseType(typeof(UserInfo), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        private ObjectResult GetErrorResult(GetUserThreadsInfoError error)
        {
            return error switch
            {
                GetUserThreadsInfoError.NotFound => NotFound(new ErrorResponse("User not found")),
                GetUserThreadsInfoError.InvalidPaginationArguments => BadRequest(new ErrorResponse("Invalid pagination arguments")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private ObjectResult GetErrorResult(GetUserThreadsCountError error)
        {
            return error switch
            {
                GetUserThreadsCountError.UserNotFound => NotFound(new ErrorResponse("User not found")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private ObjectResult GetErrorResult(UpdateProfileImageError error)
        {
            return error switch
            {
                UpdateProfileImageError.UserNotFound => NotFound(new ErrorResponse("User not found")),
                UpdateProfileImageError.ImageNotFound => BadRequest(new ErrorResponse("Image not found")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }
    }
}
