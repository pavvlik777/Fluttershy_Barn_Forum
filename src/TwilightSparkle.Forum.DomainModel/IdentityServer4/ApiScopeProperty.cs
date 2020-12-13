namespace TwilightSparkle.Forum.DomainModel.IdentityServer4
{
    public class ApiScopeProperty : Property
    {
        public int ScopeId { get; set; }
        public ApiScope Scope { get; set; }
    }
}