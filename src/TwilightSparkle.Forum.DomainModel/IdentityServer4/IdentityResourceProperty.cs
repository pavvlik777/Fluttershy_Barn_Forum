namespace TwilightSparkle.Forum.DomainModel.IdentityServer4
{
    public class IdentityResourceProperty : Property
    {
        public int IdentityResourceId { get; set; }
        public IdentityResource IdentityResource { get; set; }
    }
}