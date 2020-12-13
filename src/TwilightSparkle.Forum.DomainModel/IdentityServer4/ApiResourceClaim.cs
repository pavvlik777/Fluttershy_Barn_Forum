namespace TwilightSparkle.Forum.DomainModel.IdentityServer4
{
    public class ApiResourceClaim : UserClaim
    {
        public int ApiResourceId { get; set; }
        public ApiResource ApiResource { get; set; }
    }
}