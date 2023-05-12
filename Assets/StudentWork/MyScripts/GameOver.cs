using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void OnPlayerRequestRetry()
	{
		SceneManager.LoadScene(1);
	}

	public void OnPlayerRequestQuit()
	{
		SceneManager.LoadScene(0);
	}
}
