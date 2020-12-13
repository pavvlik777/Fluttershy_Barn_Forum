using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace TwilightSparkle.Forum.Features.Users.Models
{
    public class UserThreadsInfoRequest
    {
        [FromQuery]
        [Required]
        public int? StartIndex { get; set; }

        [FromQuery]
        [Required]
        public int? Size { get; set; }
    }
}
