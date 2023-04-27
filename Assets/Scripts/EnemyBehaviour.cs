using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
	Rigidbody2D rb;
	public float direction;
	public float movementSpeed;
	public Transform Player;
	private bool IsMoving = true;

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

				cm.StartCombat();
			}
			//CombatManager.instance.StartCombat();
		}
	}

	void Start()
	{

	}

	void FixedUpdate()
	{
		//Checks if the AI is moving
		if (IsMoving)
		{
			//If true start the coroutine
			StartCoroutine(StopMovement());
		}
		else
		{
			//If false then start movement
			StartCoroutine(StartMovement());
		}

	}

	IEnumerator StopMovement()
	{
		//Move the player for 3 secs and then set IsMoving to false
		rb.velocity = new Vector2(direction * movementSpeed, rb.velocity.y);
		yield return new WaitForSeconds(3);
		IsMoving = false;
		//Debug.Log("Finished!");
	}
	IEnumerator StartMovement()
	{
		//This does StopMovement in reverse
		rb.velocity = new Vector2(-direction * movementSpeed, rb.velocity.y);
		yield return new WaitForSeconds(3);
		IsMoving = true;
		//Debug.Log("Finished!");
	}

	void Update()
	{
		Vector3 Forward = -transform.right.normalized;
		Vector3 toOther = (Player.position - transform.position).normalized;

		Forward.z = toOther.z = 0;

		float dot = Vector2.Dot(toOther, Forward);

		Debug.DrawLine(transform.position, transform.position + Forward);

		if (Mathf.Abs(dot) < 0.5f)
		{
			Debug.Log("The other transform is behind me!");
		}
	}
}
