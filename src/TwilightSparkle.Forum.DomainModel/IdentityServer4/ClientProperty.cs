namespace TwilightSparkle.Forum.DomainModel.IdentityServer4
{
    public class ClientProperty : Property
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}