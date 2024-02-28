using UnityEngine;

public class WeaponInputManager : MonoBehaviour
{
    public MouseButton attackMouseButton = MouseButton.Left; // Przycisk myszy do ataku

    public enum MouseButton
    {
        Left = 0,
        Right = 1
    }

    // Dodana funkcja do wykrywania rodzaju rêki
    private void DetectHandType()
    {
        string parentName = transform.parent != null ? transform.parent.name : "";
        if (parentName.Equals("Hand1"))
        {
            attackMouseButton = MouseButton.Left;
        }
        else if (parentName.Equals("Hand2"))
        {
            attackMouseButton = MouseButton.Right;
        }
    }

    private void Update()
    {
        DetectHandType();
    }
}