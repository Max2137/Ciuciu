using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataScript : MonoBehaviour
{
    //public float Heaviness;
    //private static float totalHeaviness = 0f;
    //private bool isEffecting;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    isEffecting = false;
    //    totalHeaviness += Heaviness;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    PlayerMovement playerMovement = GetComponentInParent<PlayerMovement>();
    //    if (playerMovement == null)
    //    {
    //        if (isEffecting)
    //        {
    //            Debug.Log("Stopped detecting PlayerMovement. Subtracting Heaviness: " + Heaviness);
    //            totalHeaviness -= Heaviness;
    //            isEffecting = false;
    //        }
    //    }
    //    else
    //    {
    //        if (!isEffecting)
    //        {
    //            Debug.Log("Started detecting PlayerMovement. Adding Heaviness: " + Heaviness);
    //            totalHeaviness += Heaviness;
    //            isEffecting = true;
    //            playerMovement.UpdateSpeedWithTotalHeaviness(totalHeaviness);
    //        }
    //    }
    //}

    //// Metoda do zmiany Heaviness obiektu
    //public void ChangeHeaviness(float newHeaviness)
    //{
    //    totalHeaviness = totalHeaviness - Heaviness + newHeaviness;
    //    Heaviness = newHeaviness;
    //    Debug.Log("Changing Heaviness. New Total Heaviness: " + totalHeaviness);
    //}

    //// Metoda do pobrania totalHeaviness
    //public float GetTotalHeaviness()
    //{
    //    return totalHeaviness;
    //}
}