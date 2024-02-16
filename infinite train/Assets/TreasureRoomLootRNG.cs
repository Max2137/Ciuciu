using System.Collections.Generic;
using UnityEngine;

public class TreasureRoomLootRNG : MonoBehaviour
{
    public List<GameObject> LootTable; // Lista prefabów do losowania
    public List<GameObject> LootPlaces; // Lista obiektów przechowuj¹cych obiekty empty
    public bool isExclusive = false; // Opcja sprawdzaj¹ca, czy œledziæ zmiany rodzica

    private List<GameObject> spawnedLootObjects = new List<GameObject>(); // Lista przechowuj¹ca zespawnowane obiekty
    private bool hasParentChanged = false; // Flaga informuj¹ca, czy któryœ obiekt zmieni³ rodzica

    void Start()
    {
        DistributeLoot();
    }

    void DistributeLoot()
    {
        // Upewnij siê, ¿e mamy wystarczaj¹c¹ iloœæ miejsc na loot
        if (LootTable.Count <= 0 || LootPlaces.Count <= 0)
        {
            Debug.LogWarning("Brak obiektów w LootTable lub LootPlaces.");
            return;
        }

        // Kopiujemy listê, aby nie zmieniaæ oryginalnej listy LootTable
        List<GameObject> remainingLoot = new List<GameObject>(LootTable);

        // Losowanie i dystrybucja lootu na obiektach empty
        for (int i = 0; i < Mathf.Min(remainingLoot.Count, LootPlaces.Count); i++)
        {
            // Losowanie prefabu z pozosta³ych w LootTable
            int randomIndex = Random.Range(0, remainingLoot.Count);
            GameObject lootPrefab = remainingLoot[randomIndex];

            // Usuwanie wylosowanego przedmiotu z listy, aby nie móg³ siê powtórzyæ
            remainingLoot.RemoveAt(randomIndex);

            // Pobranie rotacji obiektu empty
            Quaternion emptyRotation = LootPlaces[i].transform.rotation;

            // Spawnowanie lootu na obiekcie empty z uwzglêdnieniem rotacji
            GameObject lootObject = Instantiate(lootPrefab, LootPlaces[i].transform.position, emptyRotation);
            lootObject.transform.SetParent(LootPlaces[i].transform);

            // Dodanie zespawnowanego obiektu do listy
            spawnedLootObjects.Add(lootObject);

            // Dodanie komponentu do œledzenia zmiany rodzica, jeœli isExclusive jest ustawione na true
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
            // Jeœli obiekt nie jest obiektem wyj¹tkowym (który zmieni³ rodzica), usuñ go
            if (lootObject != exceptionObject)
            {
                Destroy(lootObject);
            }
        }

        spawnedLootObjects.Clear();
        hasParentChanged = true; // Ustaw flagê informuj¹c¹, ¿e ju¿ wykonano akcjê usuniêcia obiektów
    }

    void Update()
    {
        // Sprawdzenie, czy ju¿ raz któryœ obiekt zmieni³ rodzica
        if (isExclusive && hasParentChanged)
        {
            // Jeœli tak, zatrzymaj sprawdzanie
            enabled = false;
        }

        // Dodatkowe logika lub inne operacje, które chcia³byœ wykonywaæ w Update
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

        // Przechowaj oryginalnego rodzica, gdybyœ chcia³ go przywróciæ
        lootObject.GetComponent<ExclusiveParentTracker>().originalParent = originalParent;
    }

    void Update()
    {
        // Sprawdzenie, czy rodzic zosta³ zmieniony na inny ni¿ oryginalny obiekt empty z LootPlaces
        if (transform.parent != originalParent && wasTaken == false)
        {
            Debug.Log("Obiekt z LootTable o nazwie " + gameObject.name + " zmieni³ rodzica!");

            lootRNG.RemoveRemainingLootObjects(gameObject);
            wasTaken = true;
        }
    }
}