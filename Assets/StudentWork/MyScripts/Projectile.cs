using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public GameObject projectile;
	public float launchVelocity = 700f;
	public Vector2 Force;
	//private EnemyBehaviour EnemyBehaviour;
	public int ProjectileDamage;
	void Start()
    {
		projectile.GetComponent<Rigidbody2D>().AddForce(Force);
	}

	private void Awake()
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy")
		{
			Debug.Log("WAP");
			EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();

			enemyBehaviour.EnemyHealth -= ProjectileDamage;
			Debug.Log(enemyBehaviour.EnemyHealth);

			Destroy(this);
		}
	}
}
