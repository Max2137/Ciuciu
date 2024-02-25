//using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPicking : MonoBehaviour
{
    public float interactionDistance = 2f;
    private Transform heldItem;
    private Collider[] originalColliders; // Dodano pole do przechowywania pierwotnych colliderów trzymanego obiektu

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance);
            Transform nearestObject = null;
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Item"))
                {
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
                if (heldItem != null)
                {
                    // Przywracanie colliderów, gdy trzymany obiekt jest odk³adany
                    EnableColliders(heldItem.gameObject);
                    DropItem();
                }

                PickUpItem(nearestObject);
                // Wy³¹czanie colliderów po podniesieniu nowego obiektu
                DisableColliders(nearestObject.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && heldItem != null)
        {
            // Przywracanie colliderów przedmiotu przed odk³adaniem
            EnableColliders(heldItem.gameObject);
            DropItem();
        }
    }

    void PickUpItem(Transform item)
    {
        heldItem = item;
        Debug.Log("Podnoszenie przedmiotu: " + heldItem.name);

        // Przechowaj pierwotne collidery trzymanego obiektu
        originalColliders = heldItem.GetComponentsInChildren<Collider>();

        // Wy³¹cz collidery trzymanego obiektu
        foreach (Collider collider in originalColliders)
        {
            collider.enabled = false;
        }

        heldItem.parent = transform;
        heldItem.localPosition = Vector3.zero;
        heldItem.localRotation = Quaternion.identity;
    }

    void DropItem()
    {
        Debug.Log("Odk³adanie przedmiotu: " + heldItem.name);

        // Odk³adamy przedmiot
        heldItem.parent = null;

        // Przywracanie colliderów trzymanego obiektu
        EnableColliders(heldItem.gameObject);

        // Przeniesienie obiektu do sceny
        MoveObjectToScene(heldItem.gameObject);

        heldItem = null;
    }

    void EnableColliders(GameObject obj)
    {
        // Przywracanie colliderów obiektu
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }

    void DisableColliders(GameObject obj)
    {
        // Wy³¹czanie colliderów obiektu
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    void MoveObjectToScene(GameObject obj)
    {
        Scene startScene = SceneManager.GetSceneByName("SceneStart");
        if (startScene.isLoaded)
        {
            SceneManager.MoveGameObjectToScene(obj, startScene);
        }
        else
        {
            SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        }
    }
}