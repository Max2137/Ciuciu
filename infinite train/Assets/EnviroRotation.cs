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

        // SprawdŸ kierunek rotacji i interpoluj odpowiednio
        if (isRotatingToEnd)
        {
            // Rotacja z A do B
            Quaternion startQuat = Quaternion.Euler(startRotation);
            Quaternion endQuat = Quaternion.Euler(endRotation);
            transform.rotation = Quaternion.Lerp(startQuat, endQuat, rotationProgress);
        }
        else
        {
            // Rotacja z B do A
            Quaternion startQuat = Quaternion.Euler(endRotation);
            Quaternion endQuat = Quaternion.Euler(startRotation);
            transform.rotation = Quaternion.Lerp(startQuat, endQuat, rotationProgress);
        }

        // Po zakoñczeniu rotacji zmieñ kierunek i zresetuj licznik czasu
        if (rotationProgress >= 1f)
        {
            rotationProgress = 0f;  // Zresetuj postêp
            isRotatingToEnd = !isRotatingToEnd;  // Zmieñ kierunek rotacji
        }
    }
}