using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehaviourRaycast : MonoBehaviour
{
    public enum DetectionMode { Continuous, Singular }
    public DetectionMode detectionMode = DetectionMode.Continuous;

    public float raycastDistance = 10f; // Raycast distance
    public string targetTag = "TargetTag"; // Tag of the target
    public List<string> activatedScripts; // List of script names to activate
    public float waitingTime = 1f; // Waiting time before activating scripts

    private List<MonoBehaviour> scripts = new List<MonoBehaviour>(); // List of scripts to activate
    private bool isDetected = false;
    private bool isWaiting = false;
    private GameObject detectedTarget; // Detected target

    public GameObject DetectedTarget => detectedTarget; // Public property to access the detected target

    void Update()
    {
        // Create a raycast from the object's position forward
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Check if the raycast hits an object with the specified tag
        bool hitDetected = Physics.Raycast(ray, out hit, raycastDistance) && hit.collider.CompareTag(targetTag);

        if (detectionMode == DetectionMode.Continuous)
        {
            if (hitDetected && !isWaiting)
            {
                detectedTarget = hit.collider.gameObject;
                StartCoroutine(ActivateScriptsAfterDelay());
            }
            else if (!hitDetected)
            {
                detectedTarget = null;
                // Disable all effects when the object is not detected
                foreach (var effect in scripts)
                {
                    effect.enabled = false;
                }
            }
        }
        else if (detectionMode == DetectionMode.Singular)
        {
            if (hitDetected && !isDetected && !isWaiting)
            {
                detectedTarget = hit.collider.gameObject;
                StartCoroutine(ActivateScriptsAfterDelay());
                isDetected = true;
            }
            else if (!hitDetected)
            {
                detectedTarget = null;
                isDetected = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        // Set Gizmo color
        Gizmos.color = Color.red;

        // Draw the raycast in edit mode and during gameplay
        Gizmos.DrawRay(transform.position, transform.forward * raycastDistance);
    }

    void Start()
    {
        // Add all scripts to the list
        foreach (var scriptName in activatedScripts)
        {
            Type scriptType = Type.GetType(scriptName);
            if (scriptType == null)
            {
                Debug.LogError($"Script {scriptName} not found on the object.");
                continue;
            }

            MonoBehaviour effect = (MonoBehaviour)GetComponent(scriptType);
            if (effect == null)
            {
                Debug.LogError($"Script {scriptName} not found on the object.");
            }
            else
            {
                scripts.Add(effect);
            }
        }
    }

    private IEnumerator ActivateScriptsAfterDelay()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitingTime);

        foreach (var effect in scripts)
        {
            effect.enabled = true;
        }

        isWaiting = false;
    }
}