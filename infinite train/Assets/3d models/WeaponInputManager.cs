using UnityEngine;

public class WeaponInputManager : MonoBehaviour
{
    // Usuni�cie publicznej zmiennej attackMouseButton
    public MouseButton attackMouseButton = MouseButton.Left;

    public enum MouseButton
    {
        Left = 0,
        Right = 1
    }

    // Dodana funkcja do wykrywania rodzaju r�ki
    private void DetectHandType()
    {
        string parentName = transform.parent != null ? transform.parent.name : "";

        if (parentName.Equals("Hand1"))
        {
            attackMouseButton = MouseButton.Right;
        }
        else if (parentName.Equals("Hand2"))
        {
            attackMouseButton = MouseButton.Left;
        }
    }

    private void Update()
    {
        DetectHandType();

        // Pobranie lub dodanie komponentu WeaponInputManager na obiekcie
        WeaponInputManager weaponInputManagerComponent = GetComponent<WeaponInputManager>();

        if (weaponInputManagerComponent != null)
        {
            // Manipulacja zmienn� attackMouseButton w komponencie WeaponInputManager
            weaponInputManagerComponent.attackMouseButton = attackMouseButton;
            //Debug.Log(weaponInputManagerComponent.name);
        }
    }
}