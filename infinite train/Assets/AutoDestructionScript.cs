using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestructionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Zniszcz obiekt po 2 sekundach
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}