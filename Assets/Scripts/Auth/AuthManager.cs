using System;
using System.Linq;
using System.Net.Mail;
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
        EmailText.text = "";
        PasswordText.text = "";
        
        // Email
        if (string.IsNullOrEmpty(EmailInput.text))
            EmailText.text = "Email is required";
        try {
            EmailInput.text = new MailAddress(EmailInput.text).Address;
        } catch(FormatException) {
            EmailText.text = "Invalid email address";
        }
        
        // Password
        if (string.IsNullOrEmpty(PasswordInput.text))
            PasswordText.text = "Password is required";
        if (PasswordInput.text.Length < 10) 
            PasswordText.text = "Password must be at least 10 characters long";
        if (!PasswordInput.text.Any(char.IsLower))
            PasswordText.text = "Password must contain at least 1 lowercase character";
        if (!PasswordInput.text.Any(char.IsUpper))
            PasswordText.text = "Password must contain at least 1 uppercase character";
        if (!PasswordInput.text.Any(char.IsDigit)) 
            PasswordText.text = "Password must contain at least 1 digit";
        if (PasswordInput.text.All(char.IsLetterOrDigit)) 
            PasswordText.text = "Password must contain at least 1 non-alphanumeric character";
        
        if (!string.IsNullOrEmpty(EmailText.text) || !string.IsNullOrEmpty(PasswordText.text)) return false;
        
        EmailText.text = "";
        PasswordText.text = "";
        return true;
    }

    public async void LoginUser()
    {
        if (!Validate()) return;
        PostLoginRequestDto postLoginRequestDto = new PostLoginRequestDto(EmailInput.text, PasswordInput.text);
        string jsonData = JsonUtility.ToJson(postLoginRequestDto);
        var loginResult =  await ApiManagement.PerformApiCall(SessionData.Url + "/account/login", "POST", jsonData);
        if (loginResult == null)
        {
            PasswordInput.text = "Unknown error";
            return;
        }
        if (loginResult.GetAuthError() != null)
        {
            PasswordText.text = loginResult.GetAuthError();
            return;
        }
        SessionData.TokenDto = JsonUtility.FromJson<TokenDto>(loginResult.getData());
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
        var registerResult = await ApiManagement.PerformApiCall(SessionData.Url + "/account/register", "POST", jsonData);
        if (registerResult == null)
        {
            PasswordInput.text = "Unknown error";
            return;
        }
        if (registerResult.GetAuthError() != null)
        {
            PasswordText.text = registerResult.GetAuthError();
            return;
        }
        var loginResult =  await ApiManagement.PerformApiCall(SessionData.Url + "/account/login", "POST", jsonData);
        if (loginResult == null) 
        {
            PasswordInput.text = "Unknown error";
            return;
        }
        SessionData.TokenDto = JsonUtility.FromJson<TokenDto>(loginResult.getData());
        NextScene();
    }
}
