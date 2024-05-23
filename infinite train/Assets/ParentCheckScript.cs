using UnityEngine;

public class ParentCheckScript : MonoBehaviour
{
    public string parentTag = "1stSlot";

    // Method to check if this object is a child of an object with the specified tag
    public bool IsChildOfFirstSlot()
    {
        Transform parent = transform.parent;
        while (parent != null)
        {
            if (parent.CompareTag(parentTag))
            {
                return true;
            }
            parent = parent.parent;
        }
        return false;
    }
}