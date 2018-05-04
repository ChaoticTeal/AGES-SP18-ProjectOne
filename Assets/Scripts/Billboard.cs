using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour 
{
    Camera camera;


	// Use this for initialization
	void Start () 
	{
        camera = FindObjectOfType<Camera>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
            camera.transform.rotation * Vector3.up);
	}
}
