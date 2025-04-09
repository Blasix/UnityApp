using UnityEngine;


public class Object2D : MonoBehaviour
{
    public ObjectManager objectManager;

    public bool isDragging = false;

    public void Update()
    {
        if (isDragging)
            this.transform.position = GetMousePosition();
    }

    private async void OnMouseUpAsButton()
    {
        if (isDragging)
        { 
            await ObjectManager.SaveObject();
            objectManager.ShowMenu();
        }
        
        isDragging = false;
    }

    private Vector3 GetMousePosition()
    {
        Vector3 positionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        positionInWorld.z = 0;
        return positionInWorld;
    }

}
