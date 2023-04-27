using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
	//public static CombatManager instance;
	[SerializeField] GameObject player;
	[SerializeField] GameObject enemy;

    void Awake()
    {
		//if (instance != null && instance != this)
		//{
		//	Destroy(this);
		//}
		//else
		//{
		//	instance = this;
		//}
    }

    void Update()
    {
        
    }


	public void StartCombat()
	{
		player.SetActive(false);
		SceneManager.LoadSceneAsync("TurnBasedBattle", LoadSceneMode.Additive);
	}

	public void EndCombat()
	{
		player.SetActive(true);
		Destroy(enemy);
		SceneManager.UnloadSceneAsync("TurnBasedBattle");

	}
}
