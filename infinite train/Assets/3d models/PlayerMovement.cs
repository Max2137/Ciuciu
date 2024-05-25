using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float dashMultiplier = 2f;
    public float dashCooldown = 5f;
    public float dashForce = 2000f;
    public float deceleration = 8f;
    public bool isDashing = false;
    private float currentDashCooldown = 0f;
    private Rigidbody rb;

    private Collider[] colliders;

    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
    }

    void Update()
    {
        if (currentDashCooldown > 0f)
        {
            currentDashCooldown -= Time.deltaTime;
        }

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && currentDashCooldown <= 0f)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (movement != Vector3.zero)
        {
            // Tutaj mo�esz dodatkowo obs�u�y� obr�t gracza, je�li chcesz
        }

        if (isDashing)
        {
            rb.AddForce(movement * dashForce * dashMultiplier * Time.fixedDeltaTime, ForceMode.Impulse);
        }
        else
        {
            // Je�li gracz nie porusza si�, zatrzymaj go
            if (movement == Vector3.zero)
            {
                rb.velocity = Vector3.zero;
            }
            else
            {
                // W przeciwnym razie nadaj mu pr�dko�� zgodnie z wektorem ruchu
                rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);
            }
        }

        // Aktywuj/dezaktywuj kolizje w zale�no�ci od tego, czy trwa dash
        foreach (Collider collider in colliders)
        {
            //collider.enabled = !isDashing;
        }
    }



    IEnumerator Dash()
    {
        isDashing = true;
        currentDashCooldown = dashCooldown;

        yield return new WaitForSeconds(0.2f);

        isDashing = false;
    }
}