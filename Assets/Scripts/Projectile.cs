using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{
    // SerializeFields for assignment in-editor
    // Damage a projectile does
    [SerializeField]
    float damage = 10f;
    // Time a projectile should exist before despawning
    [SerializeField]
    float lifetime = 3f;

	// Use this for initialization
	void Start () 
	{
        Destroy(gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        PlayerHealth targetHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (targetHealth)
        {
            targetHealth.TakeDamage(damage);
        }
        if (gameObject != null)
            Destroy(gameObject);
    }
}
