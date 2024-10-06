using UnityEngine;

public class EnviroRotation : MonoBehaviour
{
    // Pocz¹tkowa i koñcowa rotacja obiektu
    public Vector3 startRotation;
    public Vector3 endRotation;

    // Czas trwania animacji w sekundach
    public float rotationDuration = 5f;

    // Zmienna œledz¹ca czas od rozpoczêcia rotacji
    private float rotationProgress = 0f;

    // Flaga okreœlaj¹ca kierunek rotacji: true = z A do B, false = z B do A
    private bool isRotatingToEnd = true;

    // Si³a ró¿nicy miêdzy szybkim a wolnym tempem (1 = standardowe, wiêksza wartoœæ = wiêksza ró¿nica)
    public float easeStrength = 1f;

    // Ustawienie pocz¹tkowej rotacji
    void Start()
    {
        // Ustaw pocz¹tkow¹ rotacjê na startRotation
        transform.rotation = Quaternion.Euler(startRotation);
    }

    // Aktualizacja w czasie rzeczywistym
    void Update()
    {
        // Zwiêkszaj postêp animacji
        rotationProgress += Time.deltaTime / rotationDuration;

        // Obliczenie progresji z u¿yciem funkcji sinusoidalnej oraz si³y ró¿nicy tempa (easeStrength)
        float easedProgress = Mathf.Pow(Mathf.Sin(rotationProgress * Mathf.PI * 0.5f), easeStrength);

        // SprawdŸ kierunek rotacji i interpoluj odpowiednio
        if (isRotatingToEnd)
        {
            // Rotacja z A do B
            Quaternion startQuat = Quaternion.Euler(startRotation);
            Quaternion endQuat = Quaternion.Euler(endRotation);
            transform.rotation = Quaternion.Lerp(startQuat, endQuat, easedProgress);
        }
        else
        {
            // Rotacja z B do A
            Quaternion startQuat = Quaternion.Euler(endRotation);
            Quaternion endQuat = Quaternion.Euler(startRotation);
            transform.rotation = Quaternion.Lerp(startQuat, endQuat, easedProgress);
        }

        // Po zakoñczeniu rotacji zmieñ kierunek i zresetuj licznik czasu
        if (rotationProgress >= 1f)
        {
            rotationProgress = 0f;  // Zresetuj postêp
            isRotatingToEnd = !isRotatingToEnd;  // Zmieñ kierunek rotacji
        }
    }
}