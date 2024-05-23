using UnityEngine;

public class ProjectileStandardScript : MonoBehaviour
{
    public float speed = 10f;          // Prêdkoœæ poruszania siê pocisku
    public float lifetime = 2f;        // Czas ¿ycia pocisku (czas, po którym pocisk zostanie automatycznie zniszczony)
    public float damage = 10f;         // Obra¿enia zadawane przez pocisk

    private GameObject owner;          // W³aœciciel pocisku (przeciwnik, który go wystrzeli³)

    void Start()
    {
        // Ustawienie pocz¹tkowej prêdkoœci poruszania siê pocisku
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        // Zniszczenie pocisku po okreœlonym czasie
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        // SprawdŸ, czy pocisk koliduje z obiektem innym ni¿ jego w³aœciciel
        if (other.gameObject != owner)
        {
            // SprawdŸ, czy obiekt, z którym koliduje, ma tag "Player"
            if (other.CompareTag("Player"))
            {
                // SprawdŸ, czy obiekt, z którym koliduje, ma komponent zdrowia
                UniversalHealth health = other.GetComponent<UniversalHealth>();
                if (health != null)
                {
                    // Zadaj obra¿enia obiektowi
                    health.TakeDamage(damage, owner, EDamageType.OTHER);
                }

                // Zniszcz pocisk po trafieniu
                Destroy(gameObject);
            }
        }
    }

    // Ustawienie w³aœciciela pocisku (przeciwnik, który go wystrzeli³)
    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }
}