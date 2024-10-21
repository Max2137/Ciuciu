using UnityEngine;

[ExecuteInEditMode]
public class DesaturationEffect : MonoBehaviour
{
    public Material desaturationMaterial;  // Materiał z naszym shaderem

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (desaturationMaterial != null)
        {
            // Renderujemy obraz z shaderem
            Graphics.Blit(source, destination, desaturationMaterial);
        }
        else
        {
            // Jeśli nie mamy materiału, renderujemy obraz bez zmian
            Graphics.Blit(source, destination);
        }
    }
}
