using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace TwilightSparkle.Forum.Features.Users.Models
{
    public class UpdateCurrentProfileImageRequest
    {
        [FromBody]
        [Required]
        public string ImageExternalId { get; set; }
    }
}
