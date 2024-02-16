using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPicking : MonoBehaviour
{
    public float interactionDistance = 2f;
    private Transform heldItem;

    void Update()
    {
        // Sprawdzanie, czy gracz nacisn¹³ klawisz "e"
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Sprawdzanie, czy w pobli¿u gracza znajduje siê obiekt z tagiem "Item"
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance);
            Transform nearestObject = null;
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Item"))
                {
                    // Je¿eli gracz ju¿ trzyma przedmiot, odk³adamy go
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
                // Je¿eli gracz ju¿ trzyma przedmiot, odk³adamy go
                if (heldItem != null)
                {
                    DropItem();
                }

                // Podnieœ obiekt i ustaw go jako dziecko gracza
                PickUpItem(nearestObject);
            }
        }

        // Odk³adanie przedmiotu po naciœniêciu klawisza "R"
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
        // Ustaw pozycjê i rotacjê obiektu na pozycji gracza
        heldItem.localPosition = Vector3.zero;
        heldItem.localRotation = Quaternion.identity;
    }

    void DropItem()
    {
        Debug.Log("Odk³adanie przedmiotu: " + heldItem.name);

        // Odk³adamy przedmiot
        heldItem.parent = null;

        // SprawdŸ, czy istnieje scena "SceneStart"
        Scene startScene = SceneManager.GetSceneByName("SceneStart");
        if (startScene.isLoaded)
        {
            // Je¿eli istnieje, przenieœ przedmiot do sceny "SceneStart"
            SceneManager.MoveGameObjectToScene(heldItem.gameObject, startScene);
        }
        else
        {
            // Je¿eli nie istnieje, przenieœ przedmiot do ostatnio wczytanej sceny
            SceneManager.MoveGameObjectToScene(heldItem.gameObject, SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        }

        heldItem = null;
    }
}