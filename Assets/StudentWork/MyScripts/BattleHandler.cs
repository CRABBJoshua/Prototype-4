using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{
	private static BattleHandler instance;

	public static BattleHandler GetInstance()
	{
		return instance;
	}

	public static Sprite TextureToSet;

	[SerializeField] private GameObject CharacterBattle;
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

	public HealthComponent Health;
	public EnemyHealthComponent EnemyHealthComponent;
	public GameObject Slash;

	private int RunNumber = 0;

	public void DestroyEnemyAndPlayer()
	{
		Destroy(playerCharacterBattle.gameObject);
		Destroy(enemyCharacterBattle.gameObject);
	}

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

		Debug.Log(enemyCharacterBattle.GetComponent<SpriteRenderer>().sprite);

		Health.SetMaxHealth(PlayerMaxHealth);
		EnemyHealthComponent.SetMaxHealth(EnemyMaxHealth);

		SetActiveCharacterBattle(playerCharacterBattle);
		state = State.WaitingForPlayer;
	}

	public void OnPlayerRequestAttack()
	{
		Debug.Log("attack!");
		if (state == State.WaitingForPlayer)
		{
			state = State.Busy;
			playerCharacterBattle.Attack(enemyCharacterBattle, () =>
			{
				if (activeCharacterBattle == playerCharacterBattle)
				{
					PlayerDamage = Random.Range(10, 60);
					EnemyHealth = EnemyHealth - PlayerDamage;
					EnemyHealth = Mathf.Clamp(EnemyHealth, 0, 100);
					EnemyHealthComponent.SetHealth(EnemyHealth);

					if (EnemyHealth == 0)
					{
						Debug.Log("Enemy Dead");

						CombatManager cm = FindObjectOfType<CombatManager>();
						if (cm != null)
						{
							cm.EndCombat();
						}
					}
					Debug.Log(EnemyHealth);
				}
				Invoke("ChooseNextActiveCharacter", 1);
			});
		}
	}

	public void OnPlayerRequestRangedAttack()
	{
		if (state == State.WaitingForPlayer)
		{
			state = State.Busy;
			playerCharacterBattle.GunAttack(enemyCharacterBattle, () =>
			{
				Invoke("TakeRangedDamage", 1);
				Invoke("ChooseNextActiveCharacter", 2);
			});
		}
	}
	
	public void OnPlayerRequestQuit()
	{
		CombatManager cm = FindObjectOfType<CombatManager>();
		RunNumber = Random.Range(1, 10);
		
		if(RunNumber == 5)
		{
			cm.RunningAwayFromCombat();
		}
		else
		{
			Debug.Log("Could Not Run Away");
			Invoke("ChooseNextActiveCharacter", 2);
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
		Transform characterTransform = Instantiate(CharacterBattle.gameObject, Position, Quaternion.identity).transform;
		characterTransform.gameObject.name = name;

		if (!isPlayerTeam)
		{
			characterTransform.tag = "Enemy";
		}

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
					EnemyDamage = Random.Range(10, 50);
					PlayerHealth = PlayerHealth - EnemyDamage;
					PlayerHealth = Mathf.Clamp(PlayerHealth, 0, 100);
					Health.SetHealth(PlayerHealth);

					if (PlayerHealth == 0)
					{
						Debug.Log("Player Dead");
						CombatManager cm = FindObjectOfType<CombatManager>();
						if (cm != null)
						{
							cm.GameOver();
						}
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
		if(activeCharacterBattle != null)
		{
			activeCharacterBattle.HideSelectionCircle();
		}
		activeCharacterBattle = characterBattle;
		activeCharacterBattle.ShowSelectionCircle();
	}

	public void TakeRangedDamage()
	{
		PlayerDamage = Random.Range(20, 60);
		EnemyHealth = EnemyHealth - PlayerDamage;
		EnemyHealth = Mathf.Clamp(EnemyHealth, 0, 100);
		EnemyHealthComponent.SetHealth(EnemyHealth);

		if (EnemyHealth == 0)
		{
			Debug.Log("Enemy Dead");

			CombatManager cm = FindObjectOfType<CombatManager>();
			if (cm != null)
			{
				cm.EndCombat();
			}
		}
	}
}
