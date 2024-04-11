using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float DisplayTime = 5f; // Czas wy�wietlania tekstu
    public float FadeOutTime = 2f; // Czas zanikania

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowAndFade());

        transform.SetParent(CanvasMainParenting.GetInstance().transform);
    }

    IEnumerator ShowAndFade()
    {
        Renderer renderer = GetComponent<Renderer>();
        Color originalColor = renderer.material.color;

        // Czekaj przez czas wy�wietlania
        yield return new WaitForSeconds(DisplayTime);

        float elapsedTime = 0f;

        // Zanikanie
        while (elapsedTime < FadeOutTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / FadeOutTime);
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            renderer.material.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Upewnij si�, �e ko�cowa kolor jest ca�kowicie przezroczysty
        renderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Usu� obiekt po zanikni�ciu
        Destroy(gameObject);
    }
}