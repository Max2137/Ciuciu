using UnityEngine;

public class EnemyBehaviourFollowing : MonoBehaviour
{
    public string FollowedTag = "Player"; // Tag obiektu, który bêdzie œledzony
    public float StopDistance = 2.0f; // Odleg³oœæ, w której przeciwnik przestaje pod¹¿aæ
    public float Speed = 3.0f; // Prêdkoœæ poruszania siê przeciwnika
    public bool isRotating = true; // Czy przeciwnik ma siê obracaæ w stronê œledzonego obiektu

    private Transform followedObject; // Transform œledzonego obiektu

    void Start()
    {
        // ZnajdŸ obiekt z podanym tagiem
        GameObject target = GameObject.FindGameObjectWithTag(FollowedTag);
        if (target != null)
        {
            followedObject = target.transform;
        }
        else
        {
            Debug.LogWarning("Nie znaleziono obiektu z tagiem: " + FollowedTag);
        }
    }

    void Update()
    {
        if (followedObject != null)
        {
            // Oblicz odleg³oœæ do œledzonego obiektu
            float distanceToFollowedObject = Vector3.Distance(transform.position, followedObject.position);

            if (distanceToFollowedObject > StopDistance)
            {
                // Przeciwnik pod¹¿a za œledzonym obiektem
                Vector3 direction = (followedObject.position - transform.position).normalized;
                transform.position += direction * Speed * Time.deltaTime;

                if (isRotating)
                {
                    // Obróæ przeciwnika w stronê œledzonego obiektu
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * Speed);
                }
            }
        }
    }
}