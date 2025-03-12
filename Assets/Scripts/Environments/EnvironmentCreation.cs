using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Environments
{
    public class EnvironmentCreation : MonoBehaviour
    {
        public Button CreateBTN;
        public Button BackBTN;
    
        public TMP_InputField NameInput;
        public TMP_Text NameError;
        public TMP_InputField HeightInput;
        public TMP_InputField WidthInput;
        
        void Start()
        {
            CreateBTN.onClick.AddListener(CreateEnvironment);
            BackBTN.onClick.AddListener(GoBack);
        }

        private bool Verify()
        {
            if (NameInput.text.Length <= 1 || string.IsNullOrEmpty(NameInput.text))
            {
                NameError.text = "Name has te be at least 1 character long";
                return false;
            }
            else if (NameInput.text.Length >= 25)
            {
                NameError.text = "Name has to be less than 25 characters long";
                return false;
            }
            else
            {
                NameError.text = "";
            }

            return true;
        }
        
        public async void CreateEnvironment()
        {
            if (!Verify()) return;
            Environment2DDto environment2DCreationDto = new Environment2DDto("", NameInput.text, 0, 0, SessionData.UserId);
            var result = await ApiManagement.PerformApiCall(SessionData.Url + "/Environment2D", "POST", JsonUtility.ToJson(environment2DCreationDto));
            Environment2DDto environment2DDto = JsonUtility.FromJson<Environment2DDto>(result);
            SessionData.EnvironmentId = environment2DDto.id;
            SceneManager.LoadScene("EnvironmentCreatorScene");
        }
        
        public void GoBack()
        {
            SceneManager.LoadScene("EnvironmentSelector");
        }
    }
}