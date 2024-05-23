using UnityEngine;

public class ProjectileStandardScript : MonoBehaviour
{
    public float speed = 10f;          // Pr�dko�� poruszania si� pocisku
    public float lifetime = 2f;        // Czas �ycia pocisku (czas, po kt�rym pocisk zostanie automatycznie zniszczony)
    public float damage = 10f;         // Obra�enia zadawane przez pocisk

    private GameObject owner;          // W�a�ciciel pocisku (przeciwnik, kt�ry go wystrzeli�)

    void Start()
    {
        // Ustawienie pocz�tkowej pr�dko�ci poruszania si� pocisku
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        // Zniszczenie pocisku po okre�lonym czasie
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Sprawd�, czy pocisk koliduje z obiektem innym ni� jego w�a�ciciel
        if (other.gameObject != owner)
        {
            // Sprawd�, czy obiekt, z kt�rym koliduje, ma tag "Player"
            if (other.CompareTag("Player"))
            {
                // Sprawd�, czy obiekt, z kt�rym koliduje, ma komponent zdrowia
                UniversalHealth health = other.GetComponent<UniversalHealth>();
                if (health != null)
                {
                    // Zadaj obra�enia obiektowi
                    health.TakeDamage(damage, owner, EDamageType.OTHER);
                }

                // Zniszcz pocisk po trafieniu
                Destroy(gameObject);
            }
        }
    }

    // Ustawienie w�a�ciciela pocisku (przeciwnik, kt�ry go wystrzeli�)
    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }
}