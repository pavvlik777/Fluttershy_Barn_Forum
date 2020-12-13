namespace TwilightSparkle.Forum.DomainModel.IdentityServer4
{
    public class ApiScopeClaim : UserClaim
    {
        public int ScopeId { get; set; }
        public ApiScope Scope { get; set; }
    }
}