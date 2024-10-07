using UnityEngine;

public class ItemTypeInfo : MonoBehaviour
{
    public ItemType itemType;
    public bool isRaycasting;

    public enum ItemType
    {
        Handable,
        Helmet,
        Chestplate,
        Pants,
        Boots
    }
}