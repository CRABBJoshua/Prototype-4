using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    private TopDownCharacterController characterBase;

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
			
		}
	}
}
