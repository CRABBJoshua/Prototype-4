using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public GameObject projectile;
	public float launchVelocity = 700f;
	public Vector2 Force;
    void Start()
    {
		projectile.GetComponent<Rigidbody2D>().AddForce(Force);
    }

	private void OnColliderOverlap (Collider other)
	{
		Debug.Log("Shot");
		
	}
}
