using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Objects;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour
{
    // Menu om objecten vanuit te plaatsen
    public GameObject UISideMenu;
    // Lijst met objecten die geplaatst kunnen worden die overeenkomen met de prefabs in de prefabs map
    public List<GameObject> prefabObjects;

    // Lijst met objecten die geplaatst zijn in de wereld
    private List<GameObject> placedObjects = new List<GameObject>();

    private static int ObjectID;
    private new static GameObject gameObject;
    
    async void Start()
    {
        await LoadObjects();
    }

    // Methode om een nieuw 2D object te plaatsen
    public void PlaceNewObject2D(int index)
    {
        // Verberg het zijmenu
        UISideMenu.SetActive(false);
        // Instantieer het prefab object op de positie (0,0,0) met geen rotatie
        GameObject instanceOfPrefab = Instantiate(prefabObjects[index], Vector3.zero, Quaternion.identity);
        // Haal het Object2D component op van het nieuw geplaatste object
        Object2D object2D = instanceOfPrefab.GetComponent<Object2D>();
        // Stel de objectManager van het object in op deze instantie van ObjectManager
        object2D.objectManager = this;
        // Zet de isDragging eigenschap van het object op true zodat het gesleept kan worden
        object2D.isDragging = true;
        // Voeg het object toe aan de lijst met geplaatste objecten
        placedObjects.Add(instanceOfPrefab);
        
        // ObjectID = prefabObjects[index].GetInstanceID();
        ObjectID = index;
        gameObject = instanceOfPrefab;
    }

    public static async void SaveObject()
    {
        Object2D object2D = gameObject.GetComponent<Object2D>();
        Renderer renderer = gameObject.GetComponent<Renderer>();

        Object2DDto object2DDto = new Object2DDto
        (
            "",
            ObjectID, 
            object2D.transform.position.x, 
            object2D.transform.position.y, 
            object2D.transform.localScale.x,
            object2D.transform.localScale.y,
            object2D.transform.rotation.eulerAngles.z,
            renderer != null ? renderer.sortingLayerID : 0,
            ""
        );
        
        var result = await ApiManagement.PerformApiCall(SessionData.Url + "/Environment2D/" + SessionData.EnvironmentId + "/objects", "POST", JsonUtility.ToJson(object2DDto));
        object2DDto = JsonUtility.FromJson<Object2DDto>(result.getData());
    }
    
    private async Task LoadObjects()
    {
        var result = await ApiManagement.PerformApiCall(SessionData.Url + "/Environment2D/" + SessionData.EnvironmentId + "/objects", "GET");
        List<Object2DDto> object2DDtos = JsonConvert.DeserializeObject<List<Object2DDto>>(result.getData());
        
        foreach (var object2DDto in object2DDtos)
        {
            GameObject prefab = prefabObjects[object2DDto.PrefabId];
            if (prefab != null)
            {
                GameObject instanceOfPrefab = Instantiate(prefab, new Vector3(object2DDto.PositionX, object2DDto.PositionY, 0), Quaternion.Euler(0, 0, object2DDto.RotationZ));
                instanceOfPrefab.transform.localScale = new Vector3(object2DDto.ScaleX, object2DDto.ScaleY, 1);
                Renderer renderer = instanceOfPrefab.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sortingLayerID = object2DDto.SortingLayer;
                }
                Object2D object2D = instanceOfPrefab.GetComponent<Object2D>();
                object2D.objectManager = this;
                object2D.isDragging = false;
                placedObjects.Add(instanceOfPrefab);
            }
        }
    }

    // Methode om het menu te tonen
    public void ShowMenu()
    {
        UISideMenu.SetActive(true);
    }

    // Methode om de huidige sc�ne te resetten
    public async void Reset()
    {
        // Laad de huidige sc�ne opnieuw
        await ApiManagement.PerformApiCall(SessionData.Url + "/Environment2D/" + SessionData.EnvironmentId + "/objects", "DELETE");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void GoBack()
    {
        SceneManager.LoadScene("EnvironmentSelector");
    }
}
