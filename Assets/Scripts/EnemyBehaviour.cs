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
		if(collision.tag == "Player")
		{
			
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
		//Vector3 Forward = Vector3.forward;
		//Vector3 toOther = Player.position - transform.position;

		//if(Vector3.Dot(Forward, toOther) < 0)
		//{
		//	Debug.Log("The other transform is behind me!");
		//}
    }
}
