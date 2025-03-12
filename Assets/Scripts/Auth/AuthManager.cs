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
        else
        {
            EmailText.text = "";
        }
        
        if (string.IsNullOrEmpty(PasswordInput.text))
        {
            PasswordText.text = "Password is required";
            return false;
        }
        
        else if (PasswordInput.text.Length < 10)
        {
            PasswordText.text = "Password must be at least 10 characters long";
            return false;
        }
        
        else if (!PasswordInput.text.Any(char.IsLower))
        {
            PasswordText.text = "Password must contain at least 1 lowercase character";
            return false;
        }
        
        else if (!PasswordInput.text.Any(char.IsUpper))
        {
            PasswordText.text = "Password must contain at least 1 uppercase character";
            return false;
        }
        
        else if (!PasswordInput.text.Any(char.IsDigit))
        {
            PasswordText.text = "Password must contain at least 1 digit";
            return false;
        }
        
        else if (PasswordInput.text.All(char.IsLetterOrDigit))
        {
            PasswordText.text = "Password must contain at least 1 non-alphanumeric character";
            return false;
        }

        else
        {
            PasswordText.text = "";
        }
        
        return true;
    }

    public async void LoginUser()
    {
        if (!Validate()) return;
        PostLoginRequestDto postLoginRequestDto = new PostLoginRequestDto(EmailInput.text, PasswordInput.text);
        string jsonData = JsonUtility.ToJson(postLoginRequestDto);
        var loginData =  await ApiManagement.PerformApiCall(SessionData.Url + "/account/login", "POST", jsonData);
        if (loginData == null) return;
        SessionData.postLoginResponseDto = JsonUtility.FromJson<PostLoginResponseDto>(loginData);
        SessionData.UserId = await ApiManagement.PerformApiCall(SessionData.Url + "/user/" + EmailInput.text, "GET");
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
        var r = await ApiManagement.PerformApiCall(SessionData.Url + "/account/register", "POST", jsonData);
        if (r == null) return;
        var loginData =  await ApiManagement.PerformApiCall(SessionData.Url + "/account/login", "POST", jsonData);
        if (loginData == null) return;
        SessionData.postLoginResponseDto = JsonUtility.FromJson<PostLoginResponseDto>(loginData);
        SessionData.UserId = await ApiManagement.PerformApiCall(SessionData.Url + "/user/" + EmailInput.text, "GET");
        NextScene();
    }
}
