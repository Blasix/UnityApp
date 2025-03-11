using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Environments
{
    public class EnvironmentCreation : MonoBehaviour
    {
        public Button CreateBTN;
    
        public TMP_InputField NameInput;
        public TMP_InputField HeightInput;
        public TMP_InputField WidthInput;
        
        void Start()
        {
            CreateBTN.onClick.AddListener(CreateEnvironment);
        }
        
        public async void CreateEnvironment()
        {
            Environment2DDto environment2DCreationDto = new Environment2DDto("", NameInput.text, float.Parse(HeightInput.text), float.Parse(WidthInput.text), SessionData.UserId);
            var result = await ApiManagement.PerformApiCall("https://localhost:7005/Environment2D", "POST", JsonUtility.ToJson(environment2DCreationDto));
            Environment2DDto environment2DDto = JsonUtility.FromJson<Environment2DDto>(result);
            SessionData.EnvironmentId = environment2DDto.id;
            SceneManager.LoadScene("EnvironmentCreatorScene");
        }
    }
}