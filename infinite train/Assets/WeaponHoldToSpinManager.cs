//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class WeaponHoldToSpinManager : MonoBehaviour
//{
//    public float spinSpeed = 100f;
//    public Vector3 spinAxis = Vector3.forward; // Dodajemy zmiennπ do ustawienia osi obracania
//    public int attackDamage = 10;

//    private WeaponAudioVisual weaponAudioVisual;
//    private ParentCheckScript parentCheckScript;
//    private WeaponInputManager inputManager;
//    private WeaponDetectionCollider detectionCollider;
//    private bool isSpinning = false; // Zmienna okreúlajπca, czy trwa obracanie

//    void Start()
//    {
//        weaponAudioVisual = GetComponentInChildren<WeaponAudioVisual>();
//        parentCheckScript = GetComponent<ParentCheckScript>();
//        inputManager = GetComponent<WeaponInputManager>();
//        detectionCollider = GetComponentInChildren<WeaponDetectionCollider>();

//        if (weaponAudioVisual == null)
//        {
//            Debug.LogError("WeaponAudioVisual not found on the child objects.");
//        }
//        if (parentCheckScript == null)
//        {
//            Debug.LogError("ParentCheckScript not found on the object.");
//        }
//        if (inputManager == null)
//        {
//            Debug.LogError("WeaponInputManager not found on the object.");
//        }
//        if (detectionCollider == null)
//        {
//            Debug.LogError("WeaponDetectionCollider not found on the object.");
//        }
//        else
//        {
//            detectionCollider.spinManager = this;
//        }
//    }

//    void Update()
//    {
//        if (Input.GetMouseButton((int)inputManager.attackMouseButton) && parentCheckScript.IsChildOfFirstSlot())
//        {
//            StartSpin();
//        }
//        else if (Input.GetMouseButtonUp((int)inputManager.attackMouseButton))
//        {
//            StopSpin();
//        }
//    }

//    void StartSpin()
//    {
//        if (!isSpinning)
//        {
//            StartCoroutine(SpinCoroutine());
//        }
//    }

//    void StopSpin()
//    {
//        isSpinning = false;

//        // Znajdü skrypt PlayerRotation na najwyøszym rodzicu
//        PlayerRotation playerRotationScript = transform.root.GetComponent<PlayerRotation>();
//        if (playerRotationScript != null)
//        {
//            playerRotationScript.enabled = true;
//        }
//    }

//    IEnumerator SpinCoroutine()
//    {
//        isSpinning = true;

//        // Znajdü skrypt PlayerRotation na najwyøszym rodzicu
//        PlayerRotation playerRotationScript = transform.root.GetComponent<PlayerRotation>();
//        if (playerRotationScript != null)
//        {
//            // Dezaktywuj skrypt PlayerRotation
//            playerRotationScript.enabled = false;
//        }
//        else
//        {
//            Debug.LogError("PlayerRotation script not found on the root object.");
//        }

//        while (isSpinning)
//        {
//            transform.root.Rotate(spinAxis, spinSpeed * Time.deltaTime);
//            yield return null;
//        }

//        // Aktywuj skrypt PlayerRotation po zakoÒczeniu obracania
//        if (playerRotationScript != null)
//        {
//            playerRotationScript.enabled = true;
//        }
//    }

//    public void ReportHit(GameObject enemy)
//    {
//        if (isSpinning)
//        {
//            GetComponentInParent<WeaponAttack>().DealDamage(enemy, attackDamage);
//            Debug.Log("Zadaj obraøenia");
//        }
//    }
//}