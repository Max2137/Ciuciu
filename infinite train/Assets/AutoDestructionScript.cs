using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestructionScript : MonoBehaviour
{
    public float destructionTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        // Zniszcz obiekt po 2 sekundach
        Destroy(gameObject, destructionTime);
    }

    // Update is called once per frame
    void Update()
    {

    }
}