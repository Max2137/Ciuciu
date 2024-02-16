using System.Collections.Generic;
using UnityEngine;

public class TreasureRoomLootRNG : MonoBehaviour
{
    public List<GameObject> LootTable; // Lista prefab�w do losowania
    public List<GameObject> LootPlaces; // Lista obiekt�w przechowuj�cych obiekty empty
    public bool isExclusive = false; // Opcja sprawdzaj�ca, czy �ledzi� zmiany rodzica

    private List<GameObject> spawnedLootObjects = new List<GameObject>(); // Lista przechowuj�ca zespawnowane obiekty
    private bool hasParentChanged = false; // Flaga informuj�ca, czy kt�ry� obiekt zmieni� rodzica

    void Start()
    {
        DistributeLoot();
    }

    void DistributeLoot()
    {
        // Upewnij si�, �e mamy wystarczaj�c� ilo�� miejsc na loot
        if (LootTable.Count <= 0 || LootPlaces.Count <= 0)
        {
            Debug.LogWarning("Brak obiekt�w w LootTable lub LootPlaces.");
            return;
        }

        // Kopiujemy list�, aby nie zmienia� oryginalnej listy LootTable
        List<GameObject> remainingLoot = new List<GameObject>(LootTable);

        // Losowanie i dystrybucja lootu na obiektach empty
        for (int i = 0; i < Mathf.Min(remainingLoot.Count, LootPlaces.Count); i++)
        {
            // Losowanie prefabu z pozosta�ych w LootTable
            int randomIndex = Random.Range(0, remainingLoot.Count);
            GameObject lootPrefab = remainingLoot[randomIndex];

            // Usuwanie wylosowanego przedmiotu z listy, aby nie m�g� si� powt�rzy�
            remainingLoot.RemoveAt(randomIndex);

            // Pobranie rotacji obiektu empty
            Quaternion emptyRotation = LootPlaces[i].transform.rotation;

            // Spawnowanie lootu na obiekcie empty z uwzgl�dnieniem rotacji
            GameObject lootObject = Instantiate(lootPrefab, LootPlaces[i].transform.position, emptyRotation);
            lootObject.transform.SetParent(LootPlaces[i].transform);

            // Dodanie zespawnowanego obiektu do listy
            spawnedLootObjects.Add(lootObject);

            // Dodanie komponentu do �ledzenia zmiany rodzica, je�li isExclusive jest ustawione na true
            if (isExclusive)
            {
                ExclusiveParentTracker parentTracker = lootObject.AddComponent<ExclusiveParentTracker>();
                parentTracker.Initialize(this, lootObject, LootPlaces[i].transform);
            }
        }
    }

    public void RemoveRemainingLootObjects(GameObject exceptionObject)
    {
        foreach (GameObject lootObject in spawnedLootObjects)
        {
            // Je�li obiekt nie jest obiektem wyj�tkowym (kt�ry zmieni� rodzica), usu� go
            if (lootObject != exceptionObject)
            {
                Destroy(lootObject);
            }
        }

        spawnedLootObjects.Clear();
        hasParentChanged = true; // Ustaw flag� informuj�c�, �e ju� wykonano akcj� usuni�cia obiekt�w
    }

    void Update()
    {
        // Sprawdzenie, czy ju� raz kt�ry� obiekt zmieni� rodzica
        if (isExclusive && hasParentChanged)
        {
            // Je�li tak, zatrzymaj sprawdzanie
            enabled = false;
        }

        // Dodatkowe logika lub inne operacje, kt�re chcia�by� wykonywa� w Update
    }
}

public class ExclusiveParentTracker : MonoBehaviour
{
    private TreasureRoomLootRNG lootRNG;
    private Transform originalParent;
    private bool wasTaken = false;

    public void Initialize(TreasureRoomLootRNG lootRNG, GameObject lootObject, Transform originalParent)
    {
        this.lootRNG = lootRNG;
        this.originalParent = originalParent;

        // Przechowaj oryginalnego rodzica, gdyby� chcia� go przywr�ci�
        lootObject.GetComponent<ExclusiveParentTracker>().originalParent = originalParent;
    }

    void Update()
    {
        // Sprawdzenie, czy rodzic zosta� zmieniony na inny ni� oryginalny obiekt empty z LootPlaces
        if (transform.parent != originalParent && wasTaken == false)
        {
            Debug.Log("Obiekt z LootTable o nazwie " + gameObject.name + " zmieni� rodzica!");

            lootRNG.RemoveRemainingLootObjects(gameObject);
            wasTaken = true;
        }
    }
}