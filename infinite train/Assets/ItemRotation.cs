using UnityEngine;

public class ItemRotation : MonoBehaviour
{
    void Update()
    {
        // Sprawd�, czy obiekt ma rodzica i czy jego rodzic ma tag "1stSlot"
        if (transform.parent != null && transform.parent.tag == "1stSlot")
        {
            // Pobierz pozycj� myszy na ekranie
            Vector3 mousePosition = Input.mousePosition;

            // Uzyskaj pozycj� kamery w przestrzeni �wiata gry
            Vector3 cameraWorldPosition = Camera.main.transform.position;

            // Oblicz r�nic� w pozycji kamery wzgl�dem obiektu
            Vector3 cameraToObjectOffset = transform.position - cameraWorldPosition;

            // Ustaw pozycj� myszy w przestrzeni �wiata gry, uwzgl�dniaj�c offset kamery
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Mathf.Abs(cameraToObjectOffset.y)));

            // Oblicz rotacj� tylko w osi Y w kierunku kursora
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(worldMousePosition.x - transform.position.x, 0f, worldMousePosition.z - transform.position.z));

            // Ustaw sta�� rotacj� 90 stopni w osi X i rotacj� tylko w osi Y
            transform.rotation = Quaternion.Euler(90f, targetRotation.eulerAngles.y, 0f);
        }
    }

    void OnDrawGizmos()
    {
        // Sprawd�, czy obiekt ma rodzica i czy jego rodzic ma tag "1stSlot"
        if (transform.parent != null && transform.parent.tag == "1stSlot")
        {
            // Pobierz pozycj� myszy na ekranie
            Vector3 mousePosition = Input.mousePosition;

            // Uzyskaj pozycj� kamery w przestrzeni �wiata gry
            Vector3 cameraWorldPosition = Camera.main.transform.position;

            // Oblicz r�nic� w pozycji kamery wzgl�dem obiektu
            Vector3 cameraToObjectOffset = transform.position - cameraWorldPosition;

            // Ustaw pozycj� myszy w przestrzeni �wiata gry, uwzgl�dniaj�c offset kamery
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Mathf.Abs(cameraToObjectOffset.y)));

            // Rysuj kropk� za pomoc� Gizmos
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(worldMousePosition, 0.1f);
        }
    }
}