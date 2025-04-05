using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class APIResponse
{
    public UnityWebRequest.Result APIResult {get; private set;}
    public string Data {get; private set;}
    
    public APIResponse(UnityWebRequest.Result result, string data)
    {
        APIResult = result;
        Data = data;
    }
    
    public override string ToString()
    {
        return $"Result: {APIResult}, Data: {Data}";
    }
}

public abstract class ApiManagement
{
    public static async Task<APIResponse> PerformApiCall(string url, string method, string jsonData = null)
    {
        string token = null;
        if (SessionData.TokenDto != null) token = SessionData.TokenDto.accessToken;
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
            
            APIResponse response = new APIResponse(request.result, request.downloadHandler.text);
            return response;
        }
    }
}