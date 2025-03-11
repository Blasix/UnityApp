namespace Auth
{
    public class PostLoginRequestDto
    {
        public string email;
        public string password;
        
        public PostLoginRequestDto(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }
}