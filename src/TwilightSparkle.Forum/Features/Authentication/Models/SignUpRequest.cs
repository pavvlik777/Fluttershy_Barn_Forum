using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;


namespace TwilightSparkle.Forum.Features.Authentication.Models
{
    public class SignUpRequest
    {
        [Required]
        [FromBody]
        public string Username { get; set; }

        [Required]
        [FromBody]
        public string Password { get; set; }

        [Required]
        [FromBody]
        public string PasswordConfirmation { get; set; }

        [Required]
        [FromBody]
        public string Email { get; set; }
    }
}
