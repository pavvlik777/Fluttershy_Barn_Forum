using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TwilightSparkle.Forum.Features.Authentication.Models;
using TwilightSparkle.Forum.Foundation.Authentication;

namespace TwilightSparkle.Forum.Features.Authentication
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        private readonly ILogger<AuthenticationController> _logger;


        public AuthenticationController(IAuthenticationService authenticationService,
            ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;

            _logger = logger;
        }


        [HttpPost("sign-up")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignUp([FromBody, Required] SignUpRequest request)
        {
            _logger.LogInformation($"Creating new user with username - {request.Username}, email - {request.Email}");

            var signUpResult = await _authenticationService.SignUp(request.Map());
            if (signUpResult.IsSuccess)
            {
                _logger.LogInformation($"Created new user with username - {request.Username}, email - {request.Email}");

                return Ok();
            }

            var statusCodeResult = GetErrorResult(signUpResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed to create new user with username - {request.Username} and email - {request.Email}; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed to create new user with username - {request.Username} and email - {request.Email}; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }


        private ObjectResult GetErrorResult(SignUpError errorType)
        {
            return errorType switch
            {
                SignUpError.InvalidUsername => BadRequest(new ErrorResponse("Invalid username")),
                SignUpError.DuplicateUsername => BadRequest(new ErrorResponse("Duplicate username")),
                SignUpError.InvalidPassword => BadRequest(new ErrorResponse("Invalid password")),
                SignUpError.InvalidEmail => BadRequest(new ErrorResponse("Invalid email")),
                SignUpError.DuplicateEmail => BadRequest(new ErrorResponse("Duplicate email")),
                SignUpError.PasswordAndConfirmationNotSame => BadRequest(new ErrorResponse("Password and it's confirmation are different")),
                _ => throw new ArgumentOutOfRangeException(nameof(errorType), errorType, null)
            };
        }
    }
}
