using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    // Movement speed
    [SerializeField]
    float moveSpeed = 5f;
    // Rotation speed
    [SerializeField] 
    float rotateSpeed = 5f;
    // Player number
    [SerializeField]
    int playerNumber = 1;

    // Horizontal input
    float horizontalInput;
    // Vertical input
    float verticalInput;
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
    }

    // Handle input on necessary axes
    void HandleInput()
    {
        horizontalInput = Input.GetAxis(horizontalAxis);
        verticalInput = Input.GetAxis(verticalAxis);
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
}
