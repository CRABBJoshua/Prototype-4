using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public GameObject projectile;
	public float launchVelocity = 700f;
	public Vector2 Force;
	private EnemyBehaviour EnemyBehaviour;
	public int ProjectileDamage;
    void Start()
    {
		projectile.GetComponent<Rigidbody2D>().AddForce(Force);
    }

	private void Awake()
	{
		EnemyBehaviour = GetComponent<EnemyBehaviour>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy")
		{
			EnemyBehaviour.EnemyHealth -= ProjectileDamage;
			Destroy(this);
			Debug.Log(EnemyBehaviour.EnemyHealth);
		}
	}
}
