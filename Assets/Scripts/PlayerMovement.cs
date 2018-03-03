using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Brake multiplier
    [SerializeField]
    float brakeMultiplier = .99f;
    // Time for dancing
    [SerializeField]
    float danceTime;
    // Fire cooldown
    [SerializeField]
    float fireCooldown = 2f;
    // Projectile speed
    [SerializeField]
    float fireSpeed = 5f;
    // Maximum velocity
    [SerializeField]
    float maxVelocity = 50f;
    // Movement speed
    [SerializeField]
    float moveSpeed = 5f;
    // Rotation speed
    [SerializeField]
    float rotateSpeed = 5f;
    // Projectile prefab
    [SerializeField]
    GameObject projectile;
    // Projectile spawn point
    [SerializeField]
    Transform shootPoint;

    // Can the player shoot?
    bool canShoot = true;
    // Is the player dancing?
    bool isDancing;
    // Should the player shoot?
    bool shouldShoot;
    // Horizontal input
    float horizontalInput;
    // Vertical input
    float verticalInput;
    // Active projectile
    GameObject activeProjectile;
    // Player number
    int playerNumber_UseProperty;
    // Fire button
    string fireButton;
    // Horizontal axis
    string horizontalAxis;
    // Vertical axis
    string verticalAxis;
    // Player rigidbody
    Rigidbody rigidbody;


    public int PlayerNumber
    {
        get { return playerNumber_UseProperty; }
        set { playerNumber_UseProperty = value; }
    }

	// Use this for initialization
	void Start () 
	{
        horizontalAxis = "P" + PlayerNumber + "-Horizontal";
        verticalAxis = "P" + PlayerNumber + "-Vertical";
        fireButton = "P" + PlayerNumber + "-Fire";
        rigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () 
	{
        HandleInput();
	}

    private void FixedUpdate()
    {
        if (!isDancing)
        {
            Rotate();
            Move();
            AdjustVelocity();
        }
        if (shouldShoot && canShoot)
            Fire();
    }

    // Handle input on necessary axes
    void HandleInput()
    {
        horizontalInput = Input.GetAxis(horizontalAxis);
        verticalInput = Input.GetAxis(verticalAxis);
        shouldShoot = Input.GetButtonDown(fireButton);
    }

    // Turn the player - from the Unity TANKS tutorial
    void Rotate()
    {
        float turn = horizontalInput * rotateSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.MoveRotation(rigidbody.rotation * turnRotation);
    }

    // Move the player forward - based on the Unity TANKS tutorial
    void Move()
    {
        Vector3 movement = transform.forward * verticalInput * moveSpeed * Time.fixedDeltaTime;
        rigidbody.AddForce(movement);
    }

    // Create and launch a projectile
    void Fire()
    {
        activeProjectile = Instantiate(projectile, shootPoint.position, shootPoint.transform.rotation);
        activeProjectile.transform.Rotate(Vector3.right, -90f);
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), activeProjectile.GetComponent<Collider>());
        activeProjectile.GetComponent<Rigidbody>().velocity = shootPoint.transform.forward * fireSpeed;
        StartCoroutine(ShootCooldown());
    }

    void AdjustVelocity()
    {
        if (Mathf.Sqrt(rigidbody.velocity.sqrMagnitude) >= maxVelocity)
            rigidbody.velocity *= brakeMultiplier;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<DiscoBall>() != null)
            StartCoroutine(Dance());
    }

    // Wait for cooldown
    IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireCooldown);
        canShoot = true;
    }

    IEnumerator Dance()
    {
        isDancing = true;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.left * rotateSpeed;
        yield return new WaitForSeconds(danceTime);
        rigidbody.angularVelocity = Vector3.zero;
        isDancing = false;
    }
}
