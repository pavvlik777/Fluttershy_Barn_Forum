using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TwilightSparkle.Forum.Features.Sections.Models;
using TwilightSparkle.Forum.Foundation.SectionsService;

namespace TwilightSparkle.Forum.Features.Sections
{
    [Route("api/sections")]
    [ApiController]
    public class SectionsController : Controller
    {
        private readonly ISectionsManagementService _sectionsManagementService;

        private readonly ILogger<SectionsController> _logger;


        public SectionsController(ISectionsManagementService sectionsManagementService,
            ILogger<SectionsController> logger)
        {
            _sectionsManagementService = sectionsManagementService;

            _logger = logger;
        }


        [HttpGet("info")]
        [ProducesResponseType(typeof(SectionsResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSections([FromQuery, Required] int? startIndex, [FromQuery, Required] int? size)
        {
            _logger.LogInformation("Getting sections");

            var sectionsResult = await _sectionsManagementService.GetSections(startIndex.Value, size.Value);
            if (sectionsResult.IsSuccess)
            {
                _logger.LogInformation("Successful sections retrieval");

                return Ok(new SectionsResult(sectionsResult.Value));
            }

            var statusCodeResult = GetErrorResult(sectionsResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed sections retrieval; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed sections retrieval; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [HttpGet("count")]
        [ProducesResponseType(typeof(SectionsCountResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSectionsCount()
        {
            _logger.LogInformation("Getting sections count");

            var sectionsCountResult = await _sectionsManagementService.GetSectionsCount();
            if (sectionsCountResult.IsSuccess)
            {
                _logger.LogInformation("Successful sections count retrieval");

                return Ok(new SectionsCountResult(sectionsCountResult.Value));
            }

            var statusCodeResult = GetErrorResult(sectionsCountResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed sections count retrieval; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed sections count retrieval; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [HttpGet("{sectionName}")]
        [ProducesResponseType(typeof(SectionThreadsInfoResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSectionThreads([FromRoute, Required] string sectionName,
            [FromQuery, Required] int? startIndex, [FromQuery, Required] int? size)
        {
            _logger.LogInformation($"Getting section threads for section {sectionName}");

            var sectionThreadsResult = await _sectionsManagementService.GetSectionThreads(sectionName, startIndex.Value, size.Value);
            if (sectionThreadsResult.IsSuccess)
            {
                _logger.LogInformation($"Successful sections threads retrieval for section {sectionName}");

                return Ok(new SectionThreadsInfoResult(sectionThreadsResult.Value));
            }

            var statusCodeResult = GetErrorResult(sectionThreadsResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed sections threads retrieval for section {sectionName}; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed sections threads retrieval for section {sectionName}; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }

        [HttpGet("{sectionName}/count")]
        [ProducesResponseType(typeof(SectionThreadsCountResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSectionThreadsCount([FromRoute, Required] string sectionName)
        {
            _logger.LogInformation($"Getting section threads for section {sectionName}");

            var sectionThreadsCountResult = await _sectionsManagementService.GetSectionThreadsCount(sectionName);
            if (sectionThreadsCountResult.IsSuccess)
            {
                _logger.LogInformation($"Successful sections threads retrieval for section {sectionName}");

                return Ok(new SectionThreadsCountResult(sectionThreadsCountResult.Value));
            }

            var statusCodeResult = GetErrorResult(sectionThreadsCountResult.ErrorType);
            _logger.LogWarning(statusCodeResult.StatusCode.HasValue
                ? $"Failed sections threads retrieval for section {sectionName}; Status code - {statusCodeResult.StatusCode.Value}, reason - {statusCodeResult.Value}"
                : $"Failed sections threads retrieval for section {sectionName}; Reason - {statusCodeResult.Value}");

            return statusCodeResult;
        }


        private ObjectResult GetErrorResult(GetSectionsError error)
        {
            return error switch
            {
                GetSectionsError.InvalidPaginationArguments => BadRequest(new ErrorResponse("Invalid pagination arguments")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private ObjectResult GetErrorResult(GetSectionsCountError error)
        {
            return error switch
            {
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private ObjectResult GetErrorResult(GetSectionThreadsInfoError error)
        {
            return error switch
            {
                GetSectionThreadsInfoError.InvalidSection => BadRequest(new ErrorResponse("Invalid section")),
                GetSectionThreadsInfoError.InvalidPaginationArguments => BadRequest(new ErrorResponse("Invalid pagination arguments")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private ObjectResult GetErrorResult(GetSectionThreadsCountError error)
        {
            return error switch
            {
                GetSectionThreadsCountError.InvalidSection => BadRequest(new ErrorResponse("Invalid section")),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }
    }
}
