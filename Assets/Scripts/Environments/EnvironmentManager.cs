using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Environments
{
    public class EnvironmentManager : MonoBehaviour
    {
        List<Environment2DDto> environments = new List<Environment2DDto>();
        
        public Button save1;
        public Button save2;
        public Button save3;
        public Button save4;
        public Button save5;
        
        public Button save1Delete;
        public Button save2Delete;
        public Button save3Delete;
        public Button save4Delete;
        public Button save5Delete;
        
        async void Start()
        {
            var result = await ApiManagement.PerformApiCall(SessionData.Url + "/Environment2D", "GET");
            
            if (result == null) return;
            environments = JsonConvert.DeserializeObject<List<Environment2DDto>>(result.Data);
            for (int i = 0; i < 5; i++)
            {
                string environmentName = i < environments.Count ? environments[i].name : "Empty";
                string environmentId = i < environments.Count ? environments[i].id : null;

                switch (i)
                {
                    case 0:
                        save1.GetComponentInChildren<TMP_Text>().text = environmentName;
                        save1.onClick.AddListener(() => LoadEnvironment(environmentId));
                        if (!string.IsNullOrEmpty(environmentId))
                        {
                            save1Delete.gameObject.SetActive(true);
                            save1Delete.onClick.AddListener(() => DeleteEnvironment(environmentId));
                        }
                        break;
                    case 1:
                        save2.GetComponentInChildren<TMP_Text>().text = environmentName;
                        save2.onClick.AddListener(() => LoadEnvironment(environmentId));
                        if (!string.IsNullOrEmpty(environmentId))
                        {
                            save2Delete.gameObject.SetActive(true);
                            save2Delete.onClick.AddListener(() => DeleteEnvironment(environmentId));
                        }
                        break;
                    case 2:
                        save3.GetComponentInChildren<TMP_Text>().text = environmentName;
                        save3.onClick.AddListener(() => LoadEnvironment(environmentId));
                        if (!string.IsNullOrEmpty(environmentId))
                        {
                            save3Delete.gameObject.SetActive(true);
                            save3Delete.onClick.AddListener(() => DeleteEnvironment(environmentId));
                        }
                        break;
                    case 3:
                        save4.GetComponentInChildren<TMP_Text>().text = environmentName;
                        save4.onClick.AddListener(() => LoadEnvironment(environmentId));
                        if (!string.IsNullOrEmpty(environmentId))
                        {
                            save4Delete.gameObject.SetActive(true);
                            save4Delete.onClick.AddListener(() => DeleteEnvironment(environmentId));
                        }
                        break;
                    case 4:
                        save5.GetComponentInChildren<TMP_Text>().text = environmentName;
                        save5.onClick.AddListener(() => LoadEnvironment(environmentId));
                        if (!string.IsNullOrEmpty(environmentId))
                        {
                            save5Delete.gameObject.SetActive(true);
                            save5Delete.onClick.AddListener(() => DeleteEnvironment(environmentId));
                        }
                        break;
                }
            }
        }
        
        public void GoBack()
        {
            SceneManager.LoadScene("EnvironmentSelector");
        }
        
        private void LoadEnvironment(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                SceneManager.LoadScene("EnvironmentCreationScene");
            }
            else
            {
                SessionData.EnvironmentId = id;
                SceneManager.LoadScene("EnvironmentCreatorScene");
            }
        }
        
        private async void DeleteEnvironment(string id)
        {
            await ApiManagement.PerformApiCall(SessionData.Url + "/Environment2D/" + id, "DELETE");
            SceneManager.LoadScene("EnvironmentSelector");
        }
    }
}