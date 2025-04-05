using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public abstract class ApiManagement
{
    public static async Task<string> PerformApiCall(string url, string method, string jsonData = null)
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