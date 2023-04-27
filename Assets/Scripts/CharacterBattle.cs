using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterBattle : MonoBehaviour
{
    private TopDownCharacterController characterBase;
	public GameObject Projectile;
	private State state;
	private Vector3 slideTargetPosition;
	private Action onSlideComplete;
	private GameObject SelectionCircleGameObject;

	public AnimationCurve curve;
	public float speed = 0.05f;

	private enum State
	{
		Idle,
		Sliding,
		Busy,
	}

	private void Awake()
	{
		characterBase = GetComponent<TopDownCharacterController>();
		SelectionCircleGameObject = transform.Find("Selection Circle").gameObject;
		HideSelectionCircle();
		state = State.Idle;
	}

	private void Start()
	{
		SetUp(true);
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
				//float slideSpeed = 10f;
				//transform.position += (slideTargetPosition - GetPosition()) * slideSpeed * Time.deltaTime;

				float reachedDistance = 1f;
				if(Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance)
				{
					//Arrived at slide Target Position
					//transform.position = slideTargetPosition;
					//onSlideComplete();
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
		StartCoroutine(Slide(targetCharacterBattle, onAttackComplete));
	}

	public void GunAttack(CharacterBattle targetCharacterBattle, Action onAttackComplete)
	{
		Vector3 attackDir = targetCharacterBattle.GetPosition() - GetPosition().normalized;
		GameObject Bullet = Instantiate(Projectile, transform.position, Quaternion.identity);
		Bullet.GetComponent<Projectile>().Force = attackDir * 100;
		Bullet.transform.rotation = Quaternion.FromToRotation(Vector2.up, attackDir);

		onAttackComplete();
	}

	IEnumerator Slide(CharacterBattle targetCharacterBattle, Action onAttackComplete)
	{
		Vector3 slideTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition()).normalized;
		Vector3 startingPosition = GetPosition();

		//Implement slide:
		//- save transform.position as "originalPos"
		//- Lerp from transform.position -> slideTargetPosition
		//- Lerp from slideTargetPosition -> originalPos
		//..

		Vector3 originalPos = transform.position;

		float t = 0.0f;

		while (t < 1.0f)
		{
			t += speed;
			//Lerp from transform.position -> slideTargetPosition
			transform.position = Vector3.Lerp(originalPos, slideTargetPosition, curve.Evaluate(t));
			yield return new WaitForEndOfFrame();
		}

		t = 0.0f;

		while(t < 1.0f)
		{
			t += speed;

			//Lerp from transform.position -> slideTargetPosition
			transform.position = Vector3.Lerp(slideTargetPosition, originalPos, curve.Evaluate(t));
			yield return new WaitForEndOfFrame();
		}

		onAttackComplete.Invoke();
	}

	public void HideSelectionCircle()
	{
		SelectionCircleGameObject.SetActive(false);
	}

	public void ShowSelectionCircle()
	{
		SelectionCircleGameObject.SetActive(true);
	}
}
