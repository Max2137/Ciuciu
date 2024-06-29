using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRaycastEffect : MonoBehaviour
{
    public GameObject objectToSpawn; // Obiekt do spawnowania wybrany w Inspectorze
    public int numberOfObjects = 5; // Liczba obiektów do spawnowania
    public float spawnInterval = 0.1f; // Czas pomiêdzy spawnami
    public float maxDistance = 10f; // Maksymalna odleg³oœæ, na jak¹ mog¹ byæ spawnowane obiekty
    public MousePositionScript mousePositionScript; // Referencja do skryptu pobieraj¹cego pozycjê myszy

    private List<GameObject> spawnedObjects = new List<GameObject>(); // Lista przechowuj¹ca zespawnowane obiekty

    private IEnumerator spawnEnumerator; // Referencja do enumeratora spawnowania

    private void OnEnable()
    {
        spawnEnumerator = SpawnObjects(); // Przypisanie enumeratora do zmiennej
        StartCoroutine(spawnEnumerator); // Uruchomienie enumeratora
    }

    private void OnDisable()
    {
        StopCoroutine(spawnEnumerator); // Przerwanie enumeratora
        DestroySpawnedObjects(); // Zniszczenie zespawnowanych obiektów
    }

    private IEnumerator SpawnObjects()
    {
        Vector3 mouseWorldPosition = mousePositionScript.GetMouseWorldPosition(); // Pobieramy pozycjê myszy jednorazowo
        Vector3 directionToMouse = (mouseWorldPosition - transform.position).normalized; // Obliczamy kierunek od punktu startowego do pozycji myszy
        Quaternion rotationToMouse = Quaternion.LookRotation(directionToMouse, Vector3.up); // Obliczamy rotacjê w kierunku myszy

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Obliczamy pozycjê spawnu wzd³u¿ sta³ej linii prostej, zachowuj¹c wartoœæ osi Y
            Vector3 spawnPosition = new Vector3(transform.position.x + directionToMouse.x * (maxDistance / numberOfObjects * i),
                                                transform.position.y,
                                                transform.position.z + directionToMouse.z * (maxDistance / numberOfObjects * i));

            // Spawnowanie obiektu w obliczonej pozycji
            GameObject spawnedObj = Instantiate(objectToSpawn, spawnPosition, rotationToMouse, transform);

            spawnedObjects.Add(spawnedObj); // Dodajemy zespawnowany obiekt do listy
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void DestroySpawnedObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
        spawnedObjects.Clear(); // Czyszczenie listy po usuniêciu obiektów
    }
}