﻿namespace TwilightSparkle.Forum.DomainModel.IdentityServer4
{
    public class ClientIdPRestriction
    {
        public int Id { get; set; }
        public string Provider { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}