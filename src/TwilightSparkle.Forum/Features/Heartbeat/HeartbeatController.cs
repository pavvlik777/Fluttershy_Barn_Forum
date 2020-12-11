using System;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TwilightSparkle.Forum.Features.Heartbeat
{
    /// <summary>
    /// Heartbeat operations
    /// </summary>
    [Route("api/heartbeat")]
    [ApiController]
    public class HeartbeatController : Controller
    {
        /// <summary>
        /// Returns HTTP 200 and with UTC timestamp. Can be used to verify communication to the API.
        /// </summary>
        /// <returns></returns>
        [HttpGet("date")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var currentDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            return Ok(currentDate);
        }
    }
}
