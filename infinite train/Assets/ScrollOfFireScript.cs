using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollOfFireScript : MonoBehaviour
{
    [Header("Spawning Parameters")]
    public GameObject objectPrefab;  // Prefab of the object to spawn
    public float radius = 5f;        // Radius of the circle
    public int density = 10;         // Number of objects to spawn
    public float destroyAfter = 2.5f; // Time after which objects will be destroyed

    private CooldownScript cooldownScript;
    private ParentCheckScript parentCheckScript;
    private MousePositionScript mousePositionScript;
    private WeaponInputManager inputManager;

    void Start()
    {
        cooldownScript = GetComponent<CooldownScript>();
        parentCheckScript = GetComponent<ParentCheckScript>();
        mousePositionScript = GetComponent<MousePositionScript>();
        inputManager = GetComponentInParent<WeaponInputManager>();

        if (objectPrefab == null)
        {
            Debug.LogError("objectPrefab is not assigned!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown((int)inputManager.attackMouseButton) && cooldownScript.CanSpawn() && parentCheckScript.IsChildOfFirstSlot())
        {
            Vector3 mousePosition = mousePositionScript.GetMouseWorldPosition();
            SpawnObjectsInCircle(mousePosition);
            cooldownScript.ResetCooldown();
        }
    }

    // Method to spawn objects in a circle
    void SpawnObjectsInCircle(Vector3 center)
    {
        // Ensure the y position is fixed to 0
        center.y = 0;

        for (int i = 0; i < density; i++)
        {
            // Calculate the angle for each object
            float angle = i * Mathf.PI * 2f / density;
            // Calculate the position for each object
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius + center;
            // Instantiate the object
            GameObject spawnedObject = Instantiate(objectPrefab, position, Quaternion.identity);
            Debug.Log("Spawned Object at: " + position);

            // Destroy the spawned object after destroyAfter time
            Destroy(spawnedObject, destroyAfter);
        }
    }
}