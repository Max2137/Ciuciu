using UnityEngine;

public class MousePositionScript : MonoBehaviour
{
    // Method to get the mouse position in world space
    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        if (groundPlane.Raycast(ray, out distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);
            return worldPosition;
        }
        else
        {
            // If raycast doesn't hit the ground plane, return a default position
            Debug.LogWarning("Mouse is not pointing at the ground plane");
            return Vector3.zero;
        }
    }
}