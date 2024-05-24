using UnityEngine;

public class ItemTypeInfo : MonoBehaviour
{
    public ItemType itemType;

    public enum ItemType
    {
        Handable,
        Helmet,
        Chestplate,
        Pants,
        Boots
    }
}