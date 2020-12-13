namespace TwilightSparkle.Forum.Foundation.Authentication
{
    public enum SignUpError
    {
        InvalidUsername,
        InvalidPassword,
        InvalidEmail,
        DuplicateUsername,
        DuplicateEmail,
        PasswordAndConfirmationNotSame
    }
}