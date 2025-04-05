namespace Auth
{
    public class TokenDto
    {
        public string tokenType;
        public string accessToken;
        public string refreshToken;
        public int expiresIn;

        public TokenDto(string tokenType, string accessToken, string refreshToken, int expiresIn)
        {
            this.tokenType = tokenType;
            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
            this.expiresIn = expiresIn;
        }
    }
}