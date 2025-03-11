namespace Auth
{
    public class PostLoginResponseDto
    {
        public string tokenType;
        public string accessToken;

        public PostLoginResponseDto(string tokenType, string accessToken)
        {
            this.tokenType = tokenType;
            this.accessToken = accessToken;
        }
    }
}