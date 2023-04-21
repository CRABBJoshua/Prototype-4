using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    private TopDownCharacterController characterBase;
	public GameObject Projectile;
	private State state;
	private Vector3 slideTargetPosition;
	private Action onSlideComplete;

	private enum State
	{
		Idle,
		Sliding,
		Busy,
	}

	private void Awake()
	{
		characterBase = GetComponent<TopDownCharacterController>();
	}

	private void Start()
	{
		
	}

	public void SetUp(bool isPlayerTeam)
	{
		if(isPlayerTeam)
		{
			Debug.Log("Player Sprite Loaded");
			//characterBase.GetMaterial().mainTexture = BattleHandler.GetInstance().PlayerSprite;
			characterBase.GetComponent<Renderer>().material.mainTexture = BattleHandler.GetInstance().PlayerSprite;
		}
		else
		{
			Debug.Log("Enemy Sprite Loaded");
			//characterBase.GetMaterial().mainTexture = BattleHandler.GetInstance().PlayerSprite;
			characterBase.GetComponent<Renderer>().material.mainTexture = BattleHandler.GetInstance().EnemySprite;
		}
	}

	private void Update()
	{
		switch(state)
		{
			case State.Idle:
				break;
			case State.Sliding:
				float slideSpeed = 10f;
				transform.position += (slideTargetPosition - GetPosition()) * slideSpeed * Time.deltaTime;

				float reachedDistance = 1f;
				if(Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance)
				{
					//Arrived at slide Target Position
					transform.position = slideTargetPosition;
					onSlideComplete();
				}
				break;
			case State.Busy:
				break;
		}
	}

	public Vector3 GetPosition()
	{
		return transform.position;
	}

	public void Attack(CharacterBattle targetCharacterBattle, Action onAttackComplete)
	{
		Vector3 slideTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition()).normalized;
		Vector3 startingPosition = GetPosition();

		//Slide to target
		SlideToPosition(slideTargetPosition, () =>
		{
			//Arrive at Target, attack him
			Vector3 attackDir = targetCharacterBattle.GetPosition() - GetPosition().normalized;
			//Attack completed, slide back
			SlideToPosition(startingPosition, () =>
			{
				//Slide back completed, back to idle
				onAttackComplete();
			});
		});
	}

	public void GunAttack(CharacterBattle targetCharacterBattle, Action onAttackComplete)
	{
		Vector3 attackDir = targetCharacterBattle.GetPosition() - GetPosition().normalized;
		GameObject Bullet = Instantiate(Projectile, transform.position, Quaternion.identity);
		Bullet.GetComponent<Projectile>().Force = attackDir * 100;
		Bullet.transform.rotation = Quaternion.FromToRotation(Vector2.up, attackDir);

		onAttackComplete();
	}

	private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete)
	{
		this.slideTargetPosition = slideTargetPosition;
		this.onSlideComplete = onSlideComplete;
		state = State.Sliding;
	}
}
