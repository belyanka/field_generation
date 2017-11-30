using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonsControl : MonoBehaviour {

	public void LoadScene()
	{
		SceneManager.LoadScene("MainScene");
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
