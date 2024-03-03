using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float dashMultiplier = 2f;
    public float dashCooldown = 5f;
    public float dashForce = 2000f;
    public float deceleration = 8f;
    private bool isDashing = false;
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
            Quaternion toRotation = Quaternion.LookRotation(movement.normalized, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, toRotation, Time.deltaTime * 1000f));
        }

        if (isDashing)
        {
            rb.AddForce(movement * dashForce * dashMultiplier * Time.fixedDeltaTime, ForceMode.Impulse);
        }
        else
        {
            rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);
        }

        if (movement == Vector3.zero)
        {
            rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0f, deceleration * Time.fixedDeltaTime),
                rb.velocity.y,
                Mathf.Lerp(rb.velocity.z, 0f, deceleration * Time.fixedDeltaTime));

            rb.angularVelocity = Vector3.zero;
        }

        // Aktywuj/dezaktywuj kolizje w zale¿noœci od tego, czy trwa dash
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