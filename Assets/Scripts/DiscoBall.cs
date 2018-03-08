using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBall : MonoBehaviour 
{
    // SerializeFields
    // Disco ball lifetime
    [SerializeField]
    float discoBallLifetime = 2f;
    // Rotation speed
    [SerializeField]
    float rotateSpeed;

    // Private fields
    // Rigidbody
    Rigidbody rigidbody;

	// Use this for initialization
	void Start () 
	{
        rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject, discoBallLifetime);
	}
	
	void FixedUpdate () 
	{
        Rotate();
	}

    void Rotate()
    {
        float turn = rotateSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rigidbody.MoveRotation(rigidbody.rotation * turnRotation);
    }

}
