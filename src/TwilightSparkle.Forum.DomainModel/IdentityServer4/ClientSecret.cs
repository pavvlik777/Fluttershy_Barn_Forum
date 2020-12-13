namespace TwilightSparkle.Forum.DomainModel.IdentityServer4
{
    public class ClientSecret : Secret
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}