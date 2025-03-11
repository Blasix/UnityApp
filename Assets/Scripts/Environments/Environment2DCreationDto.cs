namespace Environments
{
    public class Environment2DCreationDto
    {
        public string name;
        public float maxHeight;
        public float maxLength;
        public string userId;
        
        public Environment2DCreationDto(string name, float maxHeight, float maxLength, string userId)
        {
            this.name = name;
            this.maxHeight = maxHeight;
            this.maxLength = maxLength;
            this.userId = userId;
        }
    }
}