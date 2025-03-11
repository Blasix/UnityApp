namespace Auth
{
    public class PostRegisterRequestDto
    {
        public string email;
        public string password;
        
        public PostRegisterRequestDto(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }
}