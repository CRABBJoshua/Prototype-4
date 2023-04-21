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
	private State state;

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
		playerCharacterBattle = SpawnCharacter(true);
		enemyCharacterBattle = SpawnCharacter(false);

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
					state = State.WaitingForPlayer;
				});
			}
			else if(Keyboard.current.eKey.wasPressedThisFrame)
			{
				state = State.Busy;
				playerCharacterBattle.Attack(enemyCharacterBattle, () =>
				{
					state = State.WaitingForPlayer;
				});
			}
		}
		
	}

	private CharacterBattle SpawnCharacter(bool isPlayerTeam)
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
		CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
		characterBattle.SetUp(isPlayerTeam);

		return characterBattle;
	}
}
