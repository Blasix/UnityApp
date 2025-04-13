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

        private bool Validate()
        {
            NameError.text = "";
            
            if (NameInput.text.Length <= 1 || string.IsNullOrEmpty(NameInput.text)) 
                NameError.text = "Name has te be at least 1 character long";
            if (NameInput.text.Length >= 25)
                NameError.text = "Name has to be less than 25 characters long";
            
            if (!string.IsNullOrEmpty(NameError.text)) return false;
        
            NameError.text = "";
            return true;
        }
        
        public async void CreateEnvironment()
        {
            if (!Validate()) return;
            Environment2DDto environment2DDto = new Environment2DDto(NameInput.text, 0, 0);
            var result = await ApiManagement.PerformApiCall(SessionData.Url + "/Environment2D", "POST", JsonUtility.ToJson(environment2DDto));
            if (result == null)
            {
                NameError.text = "Unknown error";
                return;
            }
            if (result.GetEnvironmentError() != null)
            {
                NameError.text = result.GetEnvironmentError();
                return;
            }
            Environment2D environment2D = JsonUtility.FromJson<Environment2D>(result.getData());
            SessionData.Environments.Add(environment2D);
            SessionData.EnvironmentId = environment2D.id;
            SceneManager.LoadScene("EnvironmentCreatorScene");
        }
        
        public void GoBack()
        {
            SceneManager.LoadScene("EnvironmentSelector");
        }
    }
}