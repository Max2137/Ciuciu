using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRaycastEffect : MonoBehaviour
{
    public GameObject objectToSpawn; // Obiekt do spawnowania wybrany w Inspectorze
    public int numberOfObjects = 5; // Liczba obiekt�w do spawnowania
    public float spawnInterval = 0.1f; // Czas pomi�dzy spawnami
    public float maxDistance = 10f; // Maksymalna odleg�o��, na jak� mog� by� spawnowane obiekty
    public MousePositionScript mousePositionScript; // Referencja do skryptu pobieraj�cego pozycj� myszy

    private List<GameObject> spawnedObjects = new List<GameObject>(); // Lista przechowuj�ca zespawnowane obiekty

    private IEnumerator spawnEnumerator; // Referencja do enumeratora spawnowania

    private void OnEnable()
    {
        spawnEnumerator = SpawnObjects(); // Przypisanie enumeratora do zmiennej
        StartCoroutine(spawnEnumerator); // Uruchomienie enumeratora
    }

    private void OnDisable()
    {
        StopCoroutine(spawnEnumerator); // Przerwanie enumeratora
        DestroySpawnedObjects(); // Zniszczenie zespawnowanych obiekt�w
    }

    private IEnumerator SpawnObjects()
    {
        Vector3 mouseWorldPosition = mousePositionScript.GetMouseWorldPosition(); // Pobieramy pozycj� myszy jednorazowo
        Vector3 directionToMouse = (mouseWorldPosition - transform.position).normalized; // Obliczamy kierunek od punktu startowego do pozycji myszy
        Quaternion rotationToMouse = Quaternion.LookRotation(directionToMouse, Vector3.up); // Obliczamy rotacj� w kierunku myszy

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Obliczamy pozycj� spawnu wzd�u� sta�ej linii prostej, zachowuj�c warto�� osi Y
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
        spawnedObjects.Clear(); // Czyszczenie listy po usuni�ciu obiekt�w
    }
}