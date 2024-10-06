using UnityEngine;

public class EnviroRotation : MonoBehaviour
{
    // Pocz�tkowa i ko�cowa rotacja obiektu
    public Vector3 startRotation;
    public Vector3 endRotation;

    // Czas trwania animacji w sekundach
    public float rotationDuration = 5f;

    // Zmienna �ledz�ca czas od rozpocz�cia rotacji
    private float rotationProgress = 0f;

    // Flaga okre�laj�ca kierunek rotacji: true = z A do B, false = z B do A
    private bool isRotatingToEnd = true;

    // Si�a r�nicy mi�dzy szybkim a wolnym tempem (1 = standardowe, wi�ksza warto�� = wi�ksza r�nica)
    public float easeStrength = 1f;

    // Ustawienie pocz�tkowej rotacji
    void Start()
    {
        // Ustaw pocz�tkow� rotacj� na startRotation
        transform.rotation = Quaternion.Euler(startRotation);
    }

    // Aktualizacja w czasie rzeczywistym
    void Update()
    {
        // Zwi�kszaj post�p animacji
        rotationProgress += Time.deltaTime / rotationDuration;

        // Obliczenie progresji z u�yciem funkcji sinusoidalnej oraz si�y r�nicy tempa (easeStrength)
        float easedProgress = Mathf.Pow(Mathf.Sin(rotationProgress * Mathf.PI * 0.5f), easeStrength);

        // Sprawd� kierunek rotacji i interpoluj odpowiednio
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

        // Po zako�czeniu rotacji zmie� kierunek i zresetuj licznik czasu
        if (rotationProgress >= 1f)
        {
            rotationProgress = 0f;  // Zresetuj post�p
            isRotatingToEnd = !isRotatingToEnd;  // Zmie� kierunek rotacji
        }
    }
}