namespace TwilightSparkle.Forum.Features
{
    public class ErrorResponse
    {
        public ErrorResponse(string errorText)
        {
            ErrorText = errorText;
        }


        public string ErrorText { get; private set; }


        public override string ToString()
        {
            return ErrorText;
        }
    }
}
