using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void OnPlayerRequestPlay()
	{
		SceneManager.LoadScene(1);
	}

	public void OnPlayerRequestQuit()
	{

	}
}
