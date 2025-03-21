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

    private void OnMouseUpAsButton()
    {
        isDragging = false;

        if (!isDragging)
        {
            ObjectManager.SaveObject();
            objectManager.ShowMenu();
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 positionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        positionInWorld.z = 0;
        return positionInWorld;
    }

}
