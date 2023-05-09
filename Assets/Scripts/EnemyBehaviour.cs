using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;

public class EnemyBehaviour : MonoBehaviour
{
	public GameObject player;
	public float speed;

	private float distance;

	Rigidbody2D rb;
	public float direction;
	public float movementSpeed;
	public Transform Player;
	public float radius = 1f;
	private bool isChasingPlayer = false;
	public Sprite texture;
	public int EnemyHealth;

	void Awake()
	{
		//Get the rigidbody
		rb = GetComponent<Rigidbody2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			CombatManager cm = FindObjectOfType<CombatManager>();
			if (cm != null)
			{
				cm.enemy = gameObject;
				cm.StartCombat();
			}
			//CombatManager.instance.StartCombat();
		}
	}

	void Start()
	{
		StartCoroutine(EnemyBehaviourRoutine());
	}

	private void ChasePlayer()
	{
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		distance = Vector2.Distance(transform.position, player.transform.position);
		Vector2 direction = player.transform.position - transform.position;

		transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
	}

	private IEnumerator EnemyBehaviourRoutine()
	{
		while (true)
		{
			if (isChasingPlayer)
			{
				yield return new WaitForEndOfFrame();
				continue;
			}

			StartMovement();
			yield return new WaitForSeconds(2.0f);

			if (isChasingPlayer)
				continue;

			StopMovement();
			yield return new WaitForSeconds(2.0f);
		}
	}


	void StartMovement()
	{
		//Move the player for 3 secs and then set IsMoving to false
		rb.velocity = new Vector2(direction * movementSpeed, rb.velocity.y);
		transform.Rotate(Vector3.forward * 180f);
		//Debug.Log("Finished!");
	}
	void StopMovement()
	{
		//This does StopMovement in reverse
		rb.velocity = new Vector2(-direction * movementSpeed, rb.velocity.y);

		transform.Rotate(Vector3.forward * 180f);
		//Debug.Log("Finished!");
	}

	private void OnDrawGizmos()
	{
		Vector3 Forward = -transform.right.normalized;
		Vector3 toOther = (Player.position - transform.position).normalized;

		Forward.z = toOther.z = 0;

		float dot = Vector2.Dot(toOther, Forward);

		Handles.Label(transform.position + Vector3.up * 1f, $"Dot: {dot}");
		
		Gizmos.DrawLine(transform.position, transform.position + Forward);
		Gizmos.DrawLine(transform.position, transform.position + toOther);
		Gizmos.DrawWireSphere(transform.position, radius);

	}

	void Update()
	{
		Vector3 Forward = -transform.right.normalized;
		Vector3 toOther = (Player.position - transform.position).normalized;

		Forward.z = toOther.z = 0;

		float dot = Vector2.Dot(toOther, Forward);

		if(EnemyHealth <= 0)
		{
			Destroy(gameObject);
		}
		
		if (dot > 0.5f && Vector3.Distance(Player.position, transform.position) < radius)
		{
			isChasingPlayer = true;
			ChasePlayer();
		}
		else
		{
			isChasingPlayer = false;
		}
	}

	public void TakeDamage()
	{

	}
}
