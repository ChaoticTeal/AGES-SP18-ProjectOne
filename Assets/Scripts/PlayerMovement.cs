using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Dance audio clip
    [SerializeField]
    AudioClip danceClip;
    // Fire audio clip
    [SerializeField]
    AudioClip shootClip;
    // Bounce audio source
    [SerializeField]
    AudioSource bounceSource;
    // Other audio source
    [SerializeField]
    AudioSource otherSource;
    // Brake multiplier
    [SerializeField]
    float brakeMultiplier = .99f;
    // Time for dancing
    [SerializeField]
    float danceTime;
    // Spin speed modifier for dancing
    [SerializeField]
    float danceRotateMod = 100f;
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
    // Ground layer
    [SerializeField]
    int groundLayerNumber;
    // Audio clips
    [SerializeField]
    List<AudioClip> audioClips;
    // Projectile spawn point
    [SerializeField]
    Transform shootPoint;

    // Can the player shoot?
    bool canShoot_UseProperty;
    // Is the player dancing?
    bool isDancing_UseProperty;
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

    public bool IsDancing
    {
        get { return isDancing_UseProperty; }
        set { isDancing_UseProperty = value; }
    }

    public bool CanShoot
    {
        get { return canShoot_UseProperty; }
        set { canShoot_UseProperty = value; }
    }

	// Use this for initialization
	void Start () 
	{
        CanShoot = true;
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
        if (!IsDancing)
        {
            Rotate();
            Move();
            AdjustVelocity();
        }
        if (shouldShoot && CanShoot)
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
        if(otherSource != null)
        {
            otherSource.clip = shootClip;
            otherSource.Play();
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == groundLayerNumber)
        {
            if (bounceSource != null)
            {
                bounceSource.clip = audioClips[Random.Range(0, audioClips.Count)];
                bounceSource.Play();
            }
        }
    }

    // Wait for cooldown
    IEnumerator ShootCooldown()
    {
        CanShoot = false;
        yield return new WaitForSeconds(fireCooldown);
        CanShoot = true;
    }

    IEnumerator Dance()
    {
        IsDancing = true;
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddTorque(Vector3.up * danceRotateMod);
        if (otherSource != null)
        {
            otherSource.clip = danceClip;
            otherSource.Play();
        }
        yield return new WaitForSeconds(danceTime);
        rigidbody.angularVelocity = Vector3.zero;
        IsDancing = false;
    }
}
