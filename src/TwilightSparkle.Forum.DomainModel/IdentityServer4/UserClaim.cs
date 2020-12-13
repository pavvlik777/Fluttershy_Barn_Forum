namespace TwilightSparkle.Forum.DomainModel.IdentityServer4
{
    public abstract class UserClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}