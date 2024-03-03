//using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPicking : MonoBehaviour
{
    public float interactionDistance = 2f;
    private Transform heldItem;
    private Collider[] originalColliders; // Dodano pole do przechowywania pierwotnych collider�w trzymanego obiektu

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
                    // Przywracanie collider�w, gdy trzymany obiekt jest odk�adany
                    EnableColliders(heldItem.gameObject);
                    DropItem();
                }

                PickUpItem(nearestObject);
                // Wy��czanie collider�w po podniesieniu nowego obiektu
                DisableColliders(nearestObject.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && heldItem != null)
        {
            // Przywracanie collider�w przedmiotu przed odk�adaniem
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

        // Wy��cz collidery trzymanego obiektu
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
        Debug.Log("Odk�adanie przedmiotu: " + heldItem.name);

        // Odk�adamy przedmiot
        heldItem.parent = null;

        // Przywracanie collider�w trzymanego obiektu
        EnableColliders(heldItem.gameObject);

        // Przeniesienie obiektu do sceny
        MoveObjectToScene(heldItem.gameObject);

        heldItem = null;
    }

    void EnableColliders(GameObject obj)
    {
        // Przywracanie collider�w obiektu
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }

    void DisableColliders(GameObject obj)
    {
        // Wy��czanie collider�w obiektu
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