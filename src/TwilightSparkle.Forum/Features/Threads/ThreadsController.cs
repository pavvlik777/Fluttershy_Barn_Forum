using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TwilightSparkle.Forum.Features.Threads.Models;
using TwilightSparkle.Forum.Foundation.ThreadsService;

namespace TwilightSparkle.Forum.Features.Threads
{
    [Route("api/threads")]
    [ApiController]
    public class ThreadsController : Controller
    {
        private readonly IThreadsManagementService _threadsManagementService;

        private readonly ILogger<ThreadsController> _logger;


        public ThreadsController(IThreadsManagementService threadsManagementService,
            ILogger<ThreadsController> logger)
        {
            _threadsManagementService = threadsManagementService;

            _logger = logger;
        }


        [HttpGet("popular")]
        [ProducesResponseType(typeof(ThreadsInfoResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPopularThreads([FromQuery, Required] int? startIndex, [FromQuery, Required] int? size)
        {
            _logger.LogInformation("Getting popular threads");

            var popularThreadsResult = await _threadsManagementService.GetPopularThreads(startIndex.Value, size.Value);
            if (popularThreadsResult.IsSuccess)
            {
                _logger.LogInformation("Successful popular threads retrieval");

                return Ok(new ThreadsInfoResult(popularThreadsResult.Value));
            }

            var statusCodeResult = GetErrorResult(popularThreadsResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed popular threads retrieval; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed popular threads retrieval; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [HttpGet("popular/count")]
        [ProducesResponseType(typeof(ThreadsCountResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPopularThreadsCount()
        {
            _logger.LogInformation("Getting popular threads count");

            var popularThreadsCountResult = await _threadsManagementService.GetPopularThreadsCount();
            if (popularThreadsCountResult.IsSuccess)
            {
                _logger.LogInformation("Successful popular threads count retrieval");

                return Ok(new ThreadsCountResult(popularThreadsCountResult.Value));
            }

            var statusCodeResult = GetErrorResult(popularThreadsCountResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed popular threads count retrieval; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed popular threads count retrieval; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [HttpGet("{threadId}")]
        [ProducesResponseType(typeof(ThreadInfoResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetThreadInfo([FromRoute, Required] int? threadId)
        {
            _logger.LogInformation($"Getting thread info for thread id {threadId.Value}");

            var threadInfoResult = await _threadsManagementService.GetThreadInfo(threadId.Value);
            if (threadInfoResult.IsSuccess)
            {
                _logger.LogInformation($"Successful thread info retrieval for thread id {threadId.Value}");

                return Ok(new ThreadInfoResult(threadInfoResult.Value));
            }

            var statusCodeResult = GetErrorResult(threadInfoResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed thread info retrieval for thread id {threadId.Value}; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed thread info retrieval for thread id {threadId.Value}; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [HttpGet("{threadId}/comments")]
        [ProducesResponseType(typeof(ThreadCommentsInfoResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetThreadCommentsInfo([FromRoute, Required] int? threadId,
            [FromQuery, Required] int? startIndex, [FromQuery, Required] int? size)
        {
            _logger.LogInformation($"Getting thread comments for thread id {threadId.Value}");

            var threadCommentsInfo = await _threadsManagementService.GetThreadCommentsInfo(threadId.Value, startIndex.Value, size.Value);
            if (threadCommentsInfo.IsSuccess)
            {
                _logger.LogInformation($"Successful thread comments retrieval for thread id {threadId.Value}");

                return Ok(new ThreadCommentsInfoResult(threadCommentsInfo.Value));
            }

            var statusCodeResult = GetErrorResult(threadCommentsInfo.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed thread comments retrieval for thread id {threadId.Value}; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed thread comments retrieval for thread id {threadId.Value}; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [HttpGet("{threadId}/comments/count")]
        [ProducesResponseType(typeof(ThreadCommentsCountResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetThreadCommentsCount([FromRoute, Required] int? threadId)
        {
            _logger.LogInformation($"Getting thread comments count for thread id {threadId.Value}");

            var threadCommentsCount = await _threadsManagementService.GetThreadCommentsCount(threadId.Value);
            if (threadCommentsCount.IsSuccess)
            {
                _logger.LogInformation($"Successful thread comments count retrieval for thread id {threadId.Value}");

                return Ok(new ThreadCommentsCountResult(threadCommentsCount.Value));
            }

            var statusCodeResult = GetErrorResult(threadCommentsCount.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed thread comments count retrieval for thread id {threadId.Value}; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed thread comments count retrieval for thread id {threadId.Value}; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ThreadInfoResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateThread([FromBody, Required] CreateThreadRequest request)
        {
            _logger.LogInformation($"Creating thread for user with username {User.Identity.Name}");

            var createThreadResult = await _threadsManagementService.CreateThread(request.Map(User.Identity.Name));
            if (createThreadResult.IsSuccess)
            {
                _logger.LogInformation($"Created thread for user with username {User.Identity.Name}");

                return Ok(new ThreadInfoResult(createThreadResult.Value));
            }

            var statusCodeResult = GetErrorResult(createThreadResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed thread creation for user with username {User.Identity.Name}; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed thread creation for user with username {User.Identity.Name}; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [Authorize]
        [HttpPost("{threadId}/like")]
        [ProducesResponseType(typeof(ThreadInfoResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LikeThread([FromRoute, Required] int? threadId)
        {
            _logger.LogInformation($"Set Like for thread with id {threadId.Value} for user with username {User.Identity.Name}");

            var likeThreadResult = await _threadsManagementService.LikeDislikeThread(threadId.Value, User.Identity.Name, true);
            if (likeThreadResult.IsSuccess)
            {
                _logger.LogInformation($"Successful set Like for thread with id {threadId.Value} for user with username {User.Identity.Name}");

                return Ok(new ThreadInfoResult(likeThreadResult.Value));
            }

            var statusCodeResult = GetErrorResult(likeThreadResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed set Like for thread with id {threadId.Value} for user with username {User.Identity.Name}; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed set Like for thread with id {threadId.Value} for user with username {User.Identity.Name}; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [Authorize]
        [HttpPost("{threadId}/dislike")]
        [ProducesResponseType(typeof(ThreadInfoResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DislikeThread([FromRoute, Required] int? threadId)
        {
            _logger.LogInformation($"Set Dislike for thread with id {threadId.Value} for user with username {User.Identity.Name}");

            var dislikeThreadResult = await _threadsManagementService.LikeDislikeThread(threadId.Value, User.Identity.Name, false);
            if (dislikeThreadResult.IsSuccess)
            {
                _logger.LogInformation($"Successful set Dislike for thread with id {threadId.Value} for user with username {User.Identity.Name}");

                return Ok(new ThreadInfoResult(dislikeThreadResult.Value));
            }

            var statusCodeResult = GetErrorResult(dislikeThreadResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed set Dislike for thread with id {threadId.Value} for user with username {User.Identity.Name}; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed set Dislike for thread with id {threadId.Value} for user with username {User.Identity.Name}; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [Authorize]
        [HttpPost("{threadId}/comments/create")]
        [ProducesResponseType(typeof(ThreadCommentInfoResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CommentThread([FromRoute, Required] int? threadId, [FromBody, Required] CommentThreadRequest request)
        {
            _logger.LogInformation($"Comment thread with id {threadId.Value} for user with username {User.Identity.Name}");

            var threadCommentResult = await _threadsManagementService.CommentThread(request.Map(threadId.Value, User.Identity.Name));
            if (threadCommentResult.IsSuccess)
            {
                _logger.LogInformation($"Successful comment thread with id {threadId.Value} for user with username {User.Identity.Name}");

                return Ok(new ThreadCommentInfoResult(threadCommentResult.Value));
            }

            var statusCodeResult = GetErrorResult(threadCommentResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed comment thread with id {threadId.Value} for user with username {User.Identity.Name}; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed comment thread with id {threadId.Value} for user with username {User.Identity.Name}; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [Authorize]
        [HttpDelete("{threadId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteThread([FromRoute, Required] int? threadId)
        {
            _logger.LogInformation($"Deleting thread with id {threadId.Value} for user with username {User.Identity.Name}");

            var deleteThreadResult = await _threadsManagementService.DeleteThread(threadId.Value, User.Identity.Name);
            if (deleteThreadResult.IsSuccess)
            {
                _logger.LogInformation($"Deleted thread with id {threadId.Value} for user with username {User.Identity.Name}");

                return Ok();
            }

            var statusCodeResult = GetErrorResult(deleteThreadResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed thread with id {threadId.Value} deletion for user with username {User.Identity.Name}; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed thread with id {threadId.Value} deletion for user with username {User.Identity.Name}; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }


        private ObjectResult GetErrorResult(GetThreadsInfoError error)
        {
            return error switch
            {
                GetThreadsInfoError.InvalidPaginationArguments => BadRequest(new ErrorResponse("Invalid pagination arguments")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private ObjectResult GetErrorResult(GetThreadsCountError error)
        {
            return error switch
            {
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private ObjectResult GetErrorResult(GetThreadInfoError error)
        {
            return error switch
            {
                GetThreadInfoError.NotFound => NotFound(new ErrorResponse("Thread not found")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private ObjectResult GetErrorResult(CreateThreadError error)
        {
            return error switch
            {
                CreateThreadError.SectionNotFound => BadRequest(new ErrorResponse("Section not found")),
                CreateThreadError.UserNotFound => BadRequest(new ErrorResponse("User not found")),
                CreateThreadError.InvalidTitle => BadRequest(new ErrorResponse("Invalid title")),
                CreateThreadError.InvalidContent => BadRequest(new ErrorResponse("Invalid content")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private ObjectResult GetErrorResult(DeleteThreadError error)
        {
            return error switch
            {
                DeleteThreadError.UserNotFound => NotFound(new ErrorResponse("User not found")),
                DeleteThreadError.UserNotAuthor => new ObjectResult(new ErrorResponse("User have no rights to delete thread")) { StatusCode = 403 },
                DeleteThreadError.ThreadNotFound => NotFound(new ErrorResponse("Thread not found")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private ObjectResult GetErrorResult(LikeDislikeThreadError error)
        {
            return error switch
            {
                LikeDislikeThreadError.UserNotFound => NotFound(new ErrorResponse("User not found")),
                LikeDislikeThreadError.ThreadNotFound => NotFound(new ErrorResponse("Thread not found")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private ObjectResult GetErrorResult(GetThreadCommentsError error)
        {
            return error switch
            {
                GetThreadCommentsError.InvalidPaginationArguments => BadRequest(new ErrorResponse("Invalid pagination arguments")),
                GetThreadCommentsError.ThreadNotFound => NotFound(new ErrorResponse("Thread not found")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private ObjectResult GetErrorResult(GetThreadCommentsCountError error)
        {
            return error switch
            {
                GetThreadCommentsCountError.ThreadNotFound => NotFound(new ErrorResponse("Thread not found")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private ObjectResult GetErrorResult(CommentThreadError error)
        {
            return error switch
            {
                CommentThreadError.UserNotFound => NotFound(new ErrorResponse("User not found")),
                CommentThreadError.ThreadNotFound => NotFound(new ErrorResponse("Thread not found")),
                CommentThreadError.InvalidContent => BadRequest(new ErrorResponse("Invalid content")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }
    }
}
