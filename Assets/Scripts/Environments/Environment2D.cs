namespace Environments
{
    public class Environment2DDto
    {
        public string name;
        public float maxHeight;
        public float maxLength;

        public Environment2DDto(string name, float maxHeight, float maxLength)
        {
            this.name = name;
            this.maxHeight = maxHeight;
            this.maxLength = maxLength;
        }
    }
    
    public class Environment2D
    {
        public string id;
        public string name;
        public float maxHeight;
        public float maxLength;
        public string userId;
        
        public Environment2D(string id, string name, float maxHeight, float maxLength, string userId)
        {
            this.id = id;
            this.name = name;
            this.maxHeight = maxHeight;
            this.maxLength = maxLength;
            this.userId = userId;
        }
    }
}