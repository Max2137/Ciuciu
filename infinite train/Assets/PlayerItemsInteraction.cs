using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemsInteraction : MonoBehaviour
{
    public float InteractionDistance = 2f; // Odleg³oœæ, w jakiej mo¿na interagowaæ
    public string itemTag = "Item"; // Tag obiektów, z którymi mo¿na interagowaæ
    public PlayerEqScript playerEqScript; // Referencja do skryptu PlayerEqScript

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithNearestItem();
        }
    }

    void OnDrawGizmos()
    {
        // Wizualizacja zasiêgu interakcji
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, InteractionDistance);
    }

    void InteractWithNearestItem()
    {
        GameObject nearestItem = GetNearestItemWithTag(itemTag);

        if (nearestItem != null)
        {
            // Dodaj do listy i dezaktywuj obiekt
            playerEqScript.CarriedItems.Add(nearestItem);
            nearestItem.SetActive(false);

            // Debugowe komunikaty
            Debug.Log("Interakcja z obiektem: " + nearestItem.name);
            Debug.Log("Dodano do listy CarriedItems. Aktualna liczba: " + playerEqScript.CarriedItems.Count);
        }
        else
        {
            Debug.Log("Brak obiektów z tagiem '" + itemTag + "' w zasiêgu do interakcji.");
        }
    }

    GameObject GetNearestItemWithTag(string tag)
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearestItem = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject item in items)
        {
            Collider itemCollider = item.GetComponent<Collider>();
            if (itemCollider != null)
            {
                float distance = Vector3.Distance(transform.position, itemCollider.bounds.center);

                // Wizualizacja linii gizmos
                Debug.DrawLine(transform.position, itemCollider.bounds.center, Color.yellow);

                if (distance < nearestDistance && distance <= InteractionDistance)
                {
                    nearestDistance = distance;
                    nearestItem = item;
                }
            }
        }

        return nearestItem;
    }
}