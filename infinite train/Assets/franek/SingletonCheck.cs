using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SingletonCheck : MonoBehaviour
{
    private void Update()
    {
        // SprawdŸ wszystkie obiekty w scenie "DontDestroyOnLoad"
        GameObject[] dontDestroyObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> objectsToMove = new List<GameObject>();
        Scene currentScene = SceneManager.GetActiveScene();

        foreach (GameObject obj in dontDestroyObjects)
        {
            // SprawdŸ, czy obiekt nie ma skryptu SingletonInitialization
            if (obj.GetComponent<SingletonInitialization>() == null)
            {
                // SprawdŸ, czy obiekt nie jest childem obiektu z SingletonInitialization
                if (!IsChildOfObjectWithSingletonInitialization(obj.transform))
                {
                    // SprawdŸ, czy obiekt nie jest ju¿ w aktualnej scenie
                    if (obj.scene != currentScene)
                    {
                        objectsToMove.Add(obj);
                        //Debug.Log("Dodano do listy obiektów do przeniesienia: " + obj.name);
                    }
                }
                else
                {
                    //Debug.Log("Obiekt " + obj.name + " jest childem obiektu z SingletonInitialization.");
                }
            }
            else
            {
                //Debug.Log("Obiekt " + obj.name + " ma skrypt SingletonInitialization.");
            }
        }

        // Przenieœ obiekty na aktualn¹ scenê
        foreach (GameObject objToMove in objectsToMove)
        {
            SceneManager.MoveGameObjectToScene(objToMove, currentScene);
            //Debug.Log("Przeniesiono obiekt: " + objToMove.name + " do sceny: " + currentScene.name);
        }
    }

    // Funkcja sprawdzaj¹ca, czy obiekt lub którykolwiek z jego rodziców ma skrypt SingletonInitialization
    private bool IsChildOfObjectWithSingletonInitialization(Transform objTransform)
    {
        Transform parent = objTransform.parent;

        while (parent != null)
        {
            if (parent.GetComponent<SingletonInitialization>() != null)
            {
                return true;
            }

            parent = parent.parent;
        }

        return false;
    }
}