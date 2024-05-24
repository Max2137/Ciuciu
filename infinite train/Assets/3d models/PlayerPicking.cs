using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerPicking : MonoBehaviour
{
    public List<KeyCode> interactionKeys = new List<KeyCode> { KeyCode.E }; // List of keys to interact
    public float interactionDistance = 2f;
    public List<ItemTypeInfo.ItemType> allowedItemTypes; // List of allowed item types
    private Transform heldItem;
    private Collider[] originalColliders;

    void Update()
    {
        if (IsInteractionKeyPressed())
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance);
            Transform nearestObject = null;
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Item"))
                {
                    ItemTypeInfo itemTypeInfo = collider.GetComponent<ItemTypeInfo>();
                    if (itemTypeInfo == null || !allowedItemTypes.Contains(itemTypeInfo.itemType))
                    {
                        continue;
                    }

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
                    EnableColliders(heldItem.gameObject);
                    DropItem();
                }

                PickUpItem(nearestObject);
                DisableColliders(nearestObject.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && heldItem != null)
        {
            EnableColliders(heldItem.gameObject);
            DropItem();
        }
    }

    bool IsInteractionKeyPressed()
    {
        foreach (KeyCode key in interactionKeys)
        {
            if (Input.GetKeyDown(key))
            {
                return true;
            }
        }
        return false;
    }

    void PickUpItem(Transform item)
    {
        heldItem = item;
        Debug.Log("Podnoszenie przedmiotu: " + heldItem.name);

        originalColliders = heldItem.GetComponentsInChildren<Collider>();

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

        heldItem.parent = null;

        EnableColliders(heldItem.gameObject);

        MoveObjectToScene(heldItem.gameObject);

        heldItem = null;
    }

    void EnableColliders(GameObject obj)
    {
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }

    void DisableColliders(GameObject obj)
    {
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