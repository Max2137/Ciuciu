using UnityEngine;

public class ProjectileStandardScript : MonoBehaviour
{
    public float speed = 10f;          // Prêdkoœæ poruszania siê pocisku
    public float lifetime = 2f;        // Czas ¿ycia pocisku (czas, po którym pocisk zostanie automatycznie zniszczony)
    public float damage = 10f;         // Obra¿enia zadawane przez pocisk

    private GameObject owner;          // W³aœciciel pocisku (przeciwnik, który go wystrzeli³)
    private WeaponAttack weaponAttack; // Odniesienie do skryptu WeaponAttack

    public string TargetTag;

    void Start()
    {
        // Ustawienie pocz¹tkowej prêdkoœci poruszania siê pocisku
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        weaponAttack = GetComponent<WeaponAttack>();

        // Zniszczenie pocisku po okreœlonym czasie
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        // SprawdŸ, czy pocisk koliduje z obiektem innym ni¿ jego w³aœciciel
        if (other.gameObject != owner)
        {
            // SprawdŸ, czy obiekt, z którym koliduje, ma tag "Player"
            if (other.CompareTag(TargetTag))
            {
                // Zadaj obra¿enia i efekty obiektowi za pomoc¹ WeaponAttack
                if (weaponAttack != null)
                {
                    weaponAttack.DealDamage(other.gameObject, damage);
                }
                else
                {
                    Debug.LogWarning("WeaponAttack component not found on owner.");
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
        weaponAttack = owner.GetComponent<WeaponAttack>(); // Przypisanie WeaponAttack z w³aœciciela

        // SprawdŸ, czy w³aœciciel ma komponent WeaponAttack
        if (weaponAttack == null)
        {
            Debug.LogError("Owner does not have a WeaponAttack component.");
        }
    }
}