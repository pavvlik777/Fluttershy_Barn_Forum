namespace TwilightSparkle.Forum.DomainModel.IdentityServer4
{
    public class ClientRedirectUri
    {
        public int Id { get; set; }
        public string RedirectUri { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}