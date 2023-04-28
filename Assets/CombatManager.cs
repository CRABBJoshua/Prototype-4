using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
	//public static CombatManager instance;
	[SerializeField] GameObject player;
	[SerializeField] GameObject enemy;
	[SerializeField] Cinemachine.CinemachineVirtualCamera cinemachineCam;

	private Transform emptyTransform;

	void Awake()
	{
		GameObject obj = new GameObject("camera follow thing");
		emptyTransform = obj.transform;

		obj.transform.position = Vector3.zero;

		//if (instance != null && instance != this)
		//{
		//	Destroy(this);
		//}
		//else
		//{
		//	instance = this;
		//}
	}

	public void SetCameraLookatPos(Vector3 position)
	{
		emptyTransform.position = position;
		cinemachineCam.Follow = emptyTransform;
		cinemachineCam.LookAt = emptyTransform;
	}

	public void ResetCameraLookatPlayer()
	{
		cinemachineCam.Follow = player.transform;
		cinemachineCam.LookAt = player.transform;
	}

    void Update()
    {
        
    }


	public void StartCombat()
	{
		player.SetActive(false);
		enemy.SetActive(false);
		SceneManager.LoadSceneAsync("TurnBasedBattle", LoadSceneMode.Additive);
		SetCameraLookatPos(Vector3.zero);
	}

	public void EndCombat()
	{
		BattleHandler.GetInstance().DestroyEnemyAndPlayer();

		player.SetActive(true);
		Destroy(enemy);
		SceneManager.UnloadSceneAsync("TurnBasedBattle", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
		ResetCameraLookatPlayer();
	}

	public void RunningAwayFromCombat()
	{
		BattleHandler.GetInstance().DestroyEnemyAndPlayer();
		player.SetActive(true);
		enemy.SetActive(true);
		SceneManager.UnloadSceneAsync("TurnBasedBattle", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
		ResetCameraLookatPlayer();
	}
}
