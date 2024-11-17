using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    void Update()
    {
        // Pobierz pozycjê myszy na ekranie
        Vector3 mousePosition = Input.mousePosition;

        // Rzutuj promieñ z kamery do przestrzeni œwiata gry na podstawie pozycji myszy
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        // Definiuj p³aszczyznê na poziomie gracza (y = transform.position.y)
        Plane playerPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));

        // Oblicz punkt przeciêcia promienia z p³aszczyzn¹
        if (playerPlane.Raycast(ray, out float distance))
        {
            // ZnajdŸ punkt przeciêcia w przestrzeni œwiata
            Vector3 worldMousePosition = ray.GetPoint(distance);

            // Oblicz rotacjê tylko w osi Y w kierunku kursora
            Quaternion targetRotation = Quaternion.LookRotation(worldMousePosition - transform.position);

            // Zastosuj rotacjê (zachowuj¹c obrót tylko w osi Y)
            transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        }
    }

    void OnDrawGizmos()
    {
        // SprawdŸ, czy obiekt ma rodzica i czy jego rodzic ma tag "1stSlot"
        if (transform.parent != null && transform.parent.tag == "1stSlot")
        {
            // Pobierz pozycjê myszy na ekranie
            Vector3 mousePosition = Input.mousePosition;

            // Rzutuj promieñ z kamery do przestrzeni œwiata gry na podstawie pozycji myszy
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            // Definiuj p³aszczyznê na poziomie gracza (y = transform.position.y)
            Plane playerPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));

            // Oblicz punkt przeciêcia promienia z p³aszczyzn¹
            if (playerPlane.Raycast(ray, out float distance))
            {
                // ZnajdŸ punkt przeciêcia w przestrzeni œwiata
                Vector3 worldMousePosition = ray.GetPoint(distance);

                // Rysuj kropkê za pomoc¹ Gizmos
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(worldMousePosition, 0.1f);
            }
        }
    }
}