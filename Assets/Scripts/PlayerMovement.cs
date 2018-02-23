using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    // Projectile speed
    [SerializeField]
    float fireSpeed = 5f;
    // Movement speed
    [SerializeField]
    float moveSpeed = 5f;
    // Rotation speed
    [SerializeField] 
    float rotateSpeed = 5f;
    // Projectile prefab
    [SerializeField]
    GameObject projectile;
    // Player number
    [SerializeField]
    int playerNumber = 1;
    // Projectile spawn point
    [SerializeField]
    Transform shootPoint;

    // Should the player shoot?
    bool shouldShoot;
    // Horizontal input
    float horizontalInput;
    // Vertical input
    float verticalInput;
    // Active projectile
    GameObject activeProjectile;
    // Fire button
    string fireButton;
    // Horizontal axis
    string horizontalAxis;
    // Vertical axis
    string verticalAxis;
    // Player rigidbody
    Rigidbody rigidbody;
    
	// Use this for initialization
	void Start () 
	{
        horizontalAxis = "P" + playerNumber + "-Horizontal";
        verticalAxis = "P" + playerNumber + "-Vertical";
        fireButton = "P" + playerNumber + "-Fire";
        rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        HandleInput();
	}

    private void FixedUpdate()
    {
        Rotate();
        Move();
        if (shouldShoot)
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
        float turn = horizontalInput * rotateSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rigidbody.MoveRotation(rigidbody.rotation * turnRotation);
    }

    // Move the player forward - from the Unity TANKS tutorial
    void Move()
    {
        Vector3 movement = transform.forward * verticalInput * moveSpeed * Time.deltaTime;
        rigidbody.MovePosition(rigidbody.position + movement);
    }

    void Fire()
    {
        if (activeProjectile != null)
            Destroy(activeProjectile);
        activeProjectile = Instantiate(projectile, shootPoint.position, shootPoint.transform.rotation);
        activeProjectile.transform.Rotate(Vector3.right, -90f);
        activeProjectile.GetComponent<Rigidbody>().velocity = shootPoint.transform.forward * fireSpeed;
    }
}
