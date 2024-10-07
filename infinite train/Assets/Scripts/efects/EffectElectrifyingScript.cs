using UnityEngine;
using System.Collections.Generic;

public class EffectElectrifyingScript : MonoBehaviour
{
    public GameObject electrifyingEffectPrefab; // Prefab efektu elektryzuj�cego
    public float range = 10f;
    public float damage = 10f;
    public string enemyTag = "Enemy";

    private List<GameObject> alreadyTeleported = new List<GameObject>();
    private WeaponAttack weaponAttack;

    void Start()
    {
        weaponAttack = GameObject.FindObjectOfType<WeaponAttack>();
    }

    private void Update()
    {
        Electrify();
    }

    private void Electrify()
    {
        // Przeszukaj obszar
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);

        List<GameObject> enemies = new List<GameObject>();

        // Wybierz tylko obiekty z tagiem "Enemy" i kt�re nie zosta�y jeszcze teleportowane
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(enemyTag) && !alreadyTeleported.Contains(hitCollider.gameObject))
            {
                enemies.Add(hitCollider.gameObject);
            }
        }

        // Je�li nie ma wi�cej wrog�w w zasi�gu, usu� siebie
        if (enemies.Count == 0)
        {
            Destroy(gameObject);
            return;
        }

        // Znajd� najbli�szego wroga
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        // Je�li znaleziono wroga, wykonaj efekt
        if (closestEnemy != null)
        {
            // Tworzenie efektu elektryzuj�cego jako dziecko atakowanego przeciwnika, zawsze z rotacj� (-90, 0, 0)
            GameObject effect = Instantiate(electrifyingEffectPrefab, closestEnemy.transform.position, Quaternion.Euler(-90f, 0f, 0f));
            effect.transform.parent = closestEnemy.transform;


            // Dodaj teleportowanego wroga do listy, aby go nie wybiera� ponownie
            alreadyTeleported.Add(closestEnemy);

            // Zadaj obra�enia wrogiem
            weaponAttack.DealDamage(closestEnemy, damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Rysuj sfer� zasi�gu
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}