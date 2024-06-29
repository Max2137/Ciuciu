using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowingScript : MonoBehaviour
{
    public Transform target; // The object to follow
    public float posMin; // Minimum x position
    public float posMax; // Maximum x position

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            float targetX = target.position.x;
            float clampedX = Mathf.Clamp(targetX, posMin, posMax);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }
    }
}