using UnityEngine;

public class ItemRotation : MonoBehaviour
{
    void Update()
    {
        // SprawdŸ, czy obiekt ma rodzica i czy jego rodzic ma tag "1stSlot"
        if (transform.parent != null && transform.parent.tag == "1stSlot")
        {
            // Pobierz pozycjê myszy na ekranie
            Vector3 mousePosition = Input.mousePosition;

            // Uzyskaj pozycjê kamery w przestrzeni œwiata gry
            Vector3 cameraWorldPosition = Camera.main.transform.position;

            // Oblicz ró¿nicê w pozycji kamery wzglêdem obiektu
            Vector3 cameraToObjectOffset = transform.position - cameraWorldPosition;

            // Ustaw pozycjê myszy w przestrzeni œwiata gry, uwzglêdniaj¹c offset kamery
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Mathf.Abs(cameraToObjectOffset.y)));

            // Oblicz rotacjê tylko w osi Y w kierunku kursora
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(worldMousePosition.x - transform.position.x, 0f, worldMousePosition.z - transform.position.z));

            // Ustaw sta³¹ rotacjê 90 stopni w osi X i rotacjê tylko w osi Y
            transform.rotation = Quaternion.Euler(90f, targetRotation.eulerAngles.y, 0f);
        }
    }

    void OnDrawGizmos()
    {
        // SprawdŸ, czy obiekt ma rodzica i czy jego rodzic ma tag "1stSlot"
        if (transform.parent != null && transform.parent.tag == "1stSlot")
        {
            // Pobierz pozycjê myszy na ekranie
            Vector3 mousePosition = Input.mousePosition;

            // Uzyskaj pozycjê kamery w przestrzeni œwiata gry
            Vector3 cameraWorldPosition = Camera.main.transform.position;

            // Oblicz ró¿nicê w pozycji kamery wzglêdem obiektu
            Vector3 cameraToObjectOffset = transform.position - cameraWorldPosition;

            // Ustaw pozycjê myszy w przestrzeni œwiata gry, uwzglêdniaj¹c offset kamery
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Mathf.Abs(cameraToObjectOffset.y)));

            // Rysuj kropkê za pomoc¹ Gizmos
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(worldMousePosition, 0.1f);
        }
    }
}