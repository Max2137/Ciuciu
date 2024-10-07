using UnityEngine;

public class UiAdvancedActivatorScript : MonoBehaviour
{
    // Zmienna do przypisania wybranego Canvas w inspectorze
    public Canvas targetCanvas;

    void Update()
    {
        // Sprawdzenie czy klawisz Tab zosta� naci�ni�ty
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Zmienianie stanu aktywacji Canvas
            targetCanvas.gameObject.SetActive(!targetCanvas.gameObject.activeSelf);
        }
    }
}