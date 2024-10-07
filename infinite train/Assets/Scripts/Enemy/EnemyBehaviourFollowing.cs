using UnityEngine;

public class EnemyBehaviourFollowing : MonoBehaviour
{
    public string FollowedTag = "Player"; // Tag obiektu, kt�ry b�dzie �ledzony
    public float StopDistance = 2.0f; // Odleg�o��, w kt�rej przeciwnik przestaje pod��a�
    public float Speed = 3.0f; // Pr�dko�� poruszania si� przeciwnika
    public bool isRotating = true; // Czy przeciwnik ma si� obraca� w stron� �ledzonego obiektu

    private Transform followedObject; // Transform �ledzonego obiektu

    void Start()
    {
        // Znajd� obiekt z podanym tagiem
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
            // Oblicz odleg�o�� do �ledzonego obiektu
            float distanceToFollowedObject = Vector3.Distance(transform.position, followedObject.position);

            if (distanceToFollowedObject > StopDistance)
            {
                // Przeciwnik pod��a za �ledzonym obiektem
                Vector3 direction = (followedObject.position - transform.position).normalized;
                transform.position += direction * Speed * Time.deltaTime;

                if (isRotating)
                {
                    // Obr�� przeciwnika w stron� �ledzonego obiektu
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * Speed);
                }
            }
        }
    }
}