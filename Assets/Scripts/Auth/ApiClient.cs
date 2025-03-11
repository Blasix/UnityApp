using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = System.Object;


public class ApiClient : MonoBehaviour
{

    public Button RegisterBTN;
    public Button LoginBTN;
    
    public TMP_InputField EmailInput;
    public TMP_Text EmailText;
    public TMP_InputField PasswordInput;
    public TMP_Text PasswordText;
    
    // public PostLoginResponseDto postLoginResponseDto;
    public string id;
    
    public static ApiClient instance { get; private set; }
    void Awake()
    {
        // hier controleren we of er al een instantie is van deze singleton
        // als dit zo is dan hoeven we geen nieuwe aan te maken en verwijderen we deze
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }

    private void NextScene()
    {
        EmailInput.text = "";
        EmailText.text = "";
        PasswordInput.text = "";
        PasswordText.text = "";
        Debug.Log(id);
        SceneManager.LoadScene("EnvironmentSelector");
    }

    private bool Validate()
    {
        if (string.IsNullOrEmpty(EmailInput.text))
        {
            EmailText.text = "Email is required";
            return false;
        }
        
        if (string.IsNullOrEmpty(PasswordInput.text))
        {
            PasswordText.text = "Password is required";
            return false;
        }
        
        if (PasswordInput.text.Length < 10)
        {
            PasswordText.text = "Password must be at least 10 characters long";
            return false;
        }
        
        if (!PasswordInput.text.Any(char.IsLower))
        {
            PasswordText.text = "Password must contain at least 1 lowercase character";
            return false;
        }
        
        if (!PasswordInput.text.Any(char.IsUpper))
        {
            PasswordText.text = "Password must contain at least 1 uppercase character";
            return false;
        }
        
        if (!PasswordInput.text.Any(char.IsDigit))
        {
            PasswordText.text = "Password must contain at least 1 digit";
            return false;
        }
        
        if (PasswordInput.text.All(char.IsLetterOrDigit))
        {
            PasswordText.text = "Password must contain at least 1 non-alphanumeric character";
            return false;
        }
        
        EmailText.text = "";
        PasswordText.text = "";
        return true;
    }

    public async void LoginUser()
    {
        // string email = "test@blasix.com"; 
        // string password = "Test!23456";
        if (!Validate()) return;
        PostLoginRequestDto postLoginRequestDto = new PostLoginRequestDto(EmailInput.text, PasswordInput.text);
        string jsonData = JsonUtility.ToJson(postLoginRequestDto);
        var r =  await PerformApiCall("https://localhost:7005/account/login", "POST", jsonData);
        if (r == null) return;
        id = await PerformApiCall("https://localhost:7005/user/" + EmailInput.text, "GET");
        NextScene();
    }

    public async void RegisterUser()
    {
        if (!Validate()) return;
        PostRegisterRequestDto postRegisterRequestDto = new PostRegisterRequestDto(EmailInput.text, PasswordInput.text);
        string jsonData = JsonUtility.ToJson(postRegisterRequestDto);
        var r = await PerformApiCall("https://localhost:7005/account/register", "POST", jsonData);
        if (r == null) return;
        id = await PerformApiCall("https://localhost:7005/user/" + EmailInput.text, "GET");
        NextScene();
    }
    
    private async Task<string> PerformApiCall(string url, string method, string jsonData = null, string token = null)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, method))
        {
            if (!string.IsNullOrEmpty(jsonData))
            {
                byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }

            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }
            else 
            {
                // string errorjson = request.downloadHandler.text;
                // List<string> errors = new List<string>();
                
                Debug.LogError("Fout bij API-aanroep: " + request.error);
                return null;
            }
        }
    }
}
