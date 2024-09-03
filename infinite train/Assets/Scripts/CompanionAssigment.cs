using UnityEngine;

public class CompanionAssigment : MonoBehaviour
{
    public float detectionRadius = 5f; // promieñ wykrywania gracza
    public KeyCode assignKey = KeyCode.G; // klawisz przypisania
    private GameObject player;

    void Update()
    {
        if (player == null)
        {
            DetectPlayer();
        }

        if (player != null && Input.GetKeyDown(assignKey))
        {
            AssignToPlayer();
        }
    }

    void DetectPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                player = hitCollider.gameObject;
                break;
            }
        }
    }

    void AssignToPlayer()
    {
        GetComponent<CompanionFollow>().player = player.transform;
        // Mo¿esz tutaj dodaæ dodatkow¹ logikê przydzielania
        Debug.Log("Companion assigned to player: " + player.name);
    }
}