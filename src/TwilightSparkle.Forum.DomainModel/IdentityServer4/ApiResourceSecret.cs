﻿namespace TwilightSparkle.Forum.DomainModel.IdentityServer4
{
    public class ApiResourceSecret : Secret
    {
        public int ApiResourceId { get; set; }
        public ApiResource ApiResource { get; set; }
    }
}