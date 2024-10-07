using UnityEngine;

public class CompanionFollow : MonoBehaviour
{
    public Transform player;
    public float followDistance = 2f; // minimalna odleg³oœæ do zatrzymania
    public float speed = 3.5f; // prêdkoœæ poruszania siê towarzysza

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position;
            targetPosition.y = 0; // zawsze na poziomie y = 0

            float distance = Vector3.Distance(transform.position, targetPosition);
            if (distance > followDistance)
            {
                Vector3 direction = (targetPosition - transform.position).normalized;
                Vector3 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;
                rb.MovePosition(newPosition);
            }
        }
    }
}