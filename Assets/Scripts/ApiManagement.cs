using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class APIResponse
{
    private UnityWebRequest.Result result;
    private string data;
    
    public APIResponse(UnityWebRequest.Result result, string data)
    {
        this.result = result;
        this.data = data;
    }

    public string getData()
    {
        if (result == UnityWebRequest.Result.Success) return data;
        throw new Exception("API call failed: " + result);
    }
    
    public override string ToString()
    {
        return $"Result: {result}, Data: {data}";
    }
    
    public string GetAuthError()
    {
        if (result == UnityWebRequest.Result.Success) return null;
        if (data.Contains("Failed")) return "Invalid credentials";
        if (data.Contains("DuplicateEmail")) return "This email is already registered";
        return "An unknown error occurred.";
    }
}

public abstract class ApiManagement
{
    public static async Task<APIResponse> PerformApiCall(string url, string method, string jsonData = null)
    {
        Debug.Log("Performing API call to: " + url);
        string token = null;
        if (SessionData.TokenDto != null) token = SessionData.TokenDto.accessToken;
        using UnityWebRequest request = new UnityWebRequest(url, method);
        if (!string.IsNullOrEmpty(jsonData))
        {
            var jsonToSend = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
            
        if (!string.IsNullOrEmpty(token))
            request.SetRequestHeader("Authorization", "Bearer " + token);

        await request.SendWebRequest();
            
        var response = new APIResponse(request.result, request.downloadHandler.text);
        Debug.Log(response.ToString());
        return response;
    }
}