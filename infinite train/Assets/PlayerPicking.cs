using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPicking : MonoBehaviour
{
    public float interactionDistance = 2f;
    private Transform heldItem;

    void Update()
    {
        // Sprawdzanie, czy gracz nacisn�� klawisz "e"
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Sprawdzanie, czy w pobli�u gracza znajduje si� obiekt z tagiem "Item"
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance);
            Transform nearestObject = null;
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Item"))
                {
                    // Je�eli gracz ju� trzyma przedmiot, odk�adamy go
                    if (heldItem == collider.transform)
                    {
                        continue;
                    }

                    if (nearestObject == null)
                    {
                        nearestObject = collider.transform;
                        continue;
                    }

                    if (Vector3.Distance(transform.position, nearestObject.position) > Vector3.Distance(transform.position, collider.transform.position))
                    {
                        nearestObject = collider.transform;
                    }
                }
            }

            if (nearestObject != null)
            {
                // Je�eli gracz ju� trzyma przedmiot, odk�adamy go
                if (heldItem != null)
                {
                    DropItem();
                }

                // Podnie� obiekt i ustaw go jako dziecko gracza
                PickUpItem(nearestObject);
            }
        }

        // Odk�adanie przedmiotu po naci�ni�ciu klawisza "R"
        if (Input.GetKeyDown(KeyCode.R) && heldItem != null)
        {
            DropItem();
        }
    }

    void PickUpItem(Transform item)
    {
        heldItem = item;
        Debug.Log("Podnoszenie przedmiotu: " + heldItem.name);

        heldItem.parent = transform;
        // Ustaw pozycj� i rotacj� obiektu na pozycji gracza
        heldItem.localPosition = Vector3.zero;
        heldItem.localRotation = Quaternion.identity;
    }

    void DropItem()
    {
        Debug.Log("Odk�adanie przedmiotu: " + heldItem.name);

        // Odk�adamy przedmiot
        heldItem.parent = null;

        // Sprawd�, czy istnieje scena "SceneStart"
        Scene startScene = SceneManager.GetSceneByName("SceneStart");
        if (startScene.isLoaded)
        {
            // Je�eli istnieje, przenie� przedmiot do sceny "SceneStart"
            SceneManager.MoveGameObjectToScene(heldItem.gameObject, startScene);
        }
        else
        {
            // Je�eli nie istnieje, przenie� przedmiot do ostatnio wczytanej sceny
            SceneManager.MoveGameObjectToScene(heldItem.gameObject, SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        }

        heldItem = null;
    }
}