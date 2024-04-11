using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMainParenting : MonoBehaviour
{
    private static CanvasMainParenting instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static CanvasMainParenting GetInstance()
    {
        return instance;
    }
}
