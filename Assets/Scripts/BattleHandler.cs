using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleHandler : MonoBehaviour
{
	private static BattleHandler instance;

	public static BattleHandler GetInstance()
	{
		return instance;
	}

	[SerializeField] private Transform CharacterBattle;
	public Texture2D PlayerSprite;
	public Texture2D EnemySprite;

	private CharacterBattle playerCharacterBattle;
	private CharacterBattle enemyCharacterBattle;
	private CharacterBattle activeCharacterBattle;
	private State state;

	public int PlayerMaxHealth;
	public int PlayerHealth;
	public int EnemyMaxHealth;
	public int EnemyHealth;
	private int PlayerDamage;
	private int EnemyDamage;

	private enum State
	{
		WaitingForPlayer,
		Busy,

	}

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		playerCharacterBattle = SpawnCharacter(true, "player");
		enemyCharacterBattle = SpawnCharacter(false, "enemy");

		SetActiveCharacterBattle(playerCharacterBattle);
		state = State.WaitingForPlayer;
	}

	private void Update()
	{
		if (state == State.WaitingForPlayer)
		{
			if (Keyboard.current.spaceKey.wasPressedThisFrame)
			{
				state = State.Busy;
				playerCharacterBattle.GunAttack(enemyCharacterBattle, () =>
				{
					//StartCoroutine(Delay());
					Invoke("ChooseNextActiveCharacter", 2);
				});
			}
			else if(Keyboard.current.eKey.wasPressedThisFrame)
			{
				state = State.Busy;
				playerCharacterBattle.Attack(enemyCharacterBattle, () =>
				{
					if(activeCharacterBattle == playerCharacterBattle)
					{
						PlayerDamage = Random.Range(0, 100);
						EnemyHealth = EnemyHealth - PlayerDamage;
						EnemyHealth = Mathf.Clamp(EnemyHealth, 0, 100);

						if (EnemyHealth == 0)
						{
							Debug.Log("Enemy Dead");
						}
						Debug.Log(EnemyHealth);
					}
					Invoke("ChooseNextActiveCharacter", 1);
				});
			}
		}
		
	}

	private CharacterBattle SpawnCharacter(bool isPlayerTeam, string name)
	{
		Vector3 Position;
		if (isPlayerTeam)
		{
			Position = new Vector3(-5, 0);
		}
		else
		{
			Position = new Vector3( 5, 0);
		}
		Transform characterTransform = Instantiate(CharacterBattle, Position, Quaternion.identity);
		characterTransform.gameObject.name = name;

		CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
		characterBattle.SetUp(isPlayerTeam);

		return characterBattle;
	}

	private void ChooseNextActiveCharacter()
	{
		if(activeCharacterBattle == playerCharacterBattle)
		{
			SetActiveCharacterBattle(enemyCharacterBattle);
			state = State.Busy;

			enemyCharacterBattle.Attack(playerCharacterBattle, () =>
			{
				if (activeCharacterBattle == enemyCharacterBattle)
				{
					EnemyDamage = Random.Range(0, 100);
					PlayerHealth = PlayerHealth - EnemyDamage;
					PlayerHealth = Mathf.Clamp(PlayerHealth, 0, 100);

					if (PlayerHealth == 0)
					{
						Debug.Log("Player Dead");
					}
					Debug.Log(PlayerHealth);
				}
				ChooseNextActiveCharacter();
			});
		}
		else
		{
			//Debug.Log("Set back to player");
			SetActiveCharacterBattle(playerCharacterBattle);
			state = State.WaitingForPlayer;
		}
	}

	private void SetActiveCharacterBattle(CharacterBattle characterBattle)
	{
		//Debug.Log("Passed: " + characterBattle + ", Active: " + activeCharacterBattle + ", Enemy: " + enemyCharacterBattle);
		if(activeCharacterBattle != null)
		{
			activeCharacterBattle.HideSelectionCircle();
		}
		activeCharacterBattle = characterBattle;
		activeCharacterBattle.ShowSelectionCircle();
	}

	IEnumerator Delay()
	{
		yield return new WaitForSeconds(100);
	}
}
