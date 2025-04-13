namespace Objects
{
    public class Object2DDto
    {
        public string Id;
        public int PrefabId;
        public float PositionX;
        public float PositionY;
        public float ScaleX;
        public float ScaleY;
        public float RotationZ;
        public int SortingLayer;
        public string EnvironmentId;

        public Object2DDto(string id, int prefabId, float positionX, float positionY, float scaleX, float scaleY,
            float rotationZ, int sortingLayer, string environmentId)
        {
            Id = id;
            PrefabId = prefabId;
            PositionX = positionX;
            PositionY = positionY;
            ScaleX = scaleX;
            ScaleY = scaleY;
            RotationZ = rotationZ;
            SortingLayer = sortingLayer;
            EnvironmentId = environmentId;
        }
    }

    public class Object2DCreationDto
    {
        public int PrefabId;
        public float PositionX;
        public float PositionY;
        public float ScaleX;
        public float ScaleY;
        public float RotationZ;
        public int SortingLayer;
        
        public Object2DCreationDto(int prefabId, float positionX, float positionY, float scaleX, float scaleY, float rotationZ, int sortingLayer)
        {
            PrefabId = prefabId;
            PositionX = positionX;
            PositionY = positionY;
            ScaleX = scaleX;
            ScaleY = scaleY;
            RotationZ = rotationZ;
            SortingLayer = sortingLayer;
        }
    }
}