using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{
	[SerializeField] private Transform CharacterBattle;

	private void Start()
	{
		SpawnCharacter(true);
		SpawnCharacter(false);
	}

	private void SpawnCharacter(bool isPlayerTeam)
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
		Instantiate(CharacterBattle, Position, Quaternion.identity);
	}
}
