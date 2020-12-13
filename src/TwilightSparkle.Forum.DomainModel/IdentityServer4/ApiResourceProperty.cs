namespace TwilightSparkle.Forum.DomainModel.IdentityServer4
{
    public class ApiResourceProperty : Property
    {
        public int ApiResourceId { get; set; }
        public ApiResource ApiResource { get; set; }
    }
}