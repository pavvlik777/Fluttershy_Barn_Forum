using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public async Task<IActionResult> GetPopularThreads()
        {

        }

        [HttpGet("{threadId}")]
        public async Task<IActionResult> GetThreadInfo()
        {

        }

        [HttpPost("{threadId}")]
        [Authorize]
        public async Task<IActionResult> CreateThread()
        {

        }

        [HttpPost("{threadId}/like")]
        [Authorize]
        public async Task<IActionResult> LikeThread()
        {

        }

        [HttpPost("{threadId}/dislike")]
        [Authorize]
        public async Task<IActionResult> DislikeThread()
        {

        }

        [HttpPost("{threadId}/comment")]
        [Authorize]
        public async Task<IActionResult> CommentThread()
        {

        }

        [HttpDelete("{threadId}")]
        [Authorize]
        public async Task<IActionResult> DeleteThread()
        {

        }
    }
}
