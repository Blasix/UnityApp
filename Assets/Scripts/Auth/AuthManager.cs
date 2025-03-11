using System.Linq;
using Auth;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{

    public Button RegisterBTN;
    public Button LoginBTN;
    public bool isLogin = true;
    
    public TMP_InputField EmailInput;
    public TMP_Text EmailText;
    public TMP_InputField PasswordInput;
    public TMP_Text PasswordText;
    
    // public PostLoginResponseDto postLoginResponseDto;

    private void NextScene()
    {
        EmailInput.text = "";
        EmailText.text = "";
        PasswordInput.text = "";
        PasswordText.text = "";
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
        var r =  await ApiManagement.PerformApiCall("https://localhost:7005/account/login", "POST", jsonData);
        if (r == null) return;
        SessionData.UserId = await ApiManagement.PerformApiCall("https://localhost:7005/user/" + EmailInput.text, "GET");
        NextScene();
    }

    public void switchAuth()
    {
        if (isLogin)
        {
            RegisterBTN.gameObject.SetActive(true);
            LoginBTN.gameObject.SetActive(false);
            isLogin = false;
        }
        else
        {
            RegisterBTN.gameObject.SetActive(false);
            LoginBTN.gameObject.SetActive(true);
            isLogin = true;
        }
    }

    public async void RegisterUser()
    {
        if (!Validate()) return;
        PostRegisterRequestDto postRegisterRequestDto = new PostRegisterRequestDto(EmailInput.text, PasswordInput.text);
        string jsonData = JsonUtility.ToJson(postRegisterRequestDto);
        var r = await ApiManagement.PerformApiCall("https://localhost:7005/account/register", "POST", jsonData);
        if (r == null) return;
        SessionData.UserId = await ApiManagement.PerformApiCall("https://localhost:7005/user/" + EmailInput.text, "GET");
        NextScene();
    }
}
