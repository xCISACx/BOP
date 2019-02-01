using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour 
{

	public Transform canvas;
	public bool quitToMenu; 

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Pause();
		}
	}

	public void Pause() //TODO: Fix cursor not showing up in pause menu.
	{
		if (canvas.gameObject.activeInHierarchy == false)
		{
			canvas.gameObject.SetActive(true);
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); 
			Time.timeScale = 0;
		}
		else
		{
			canvas.gameObject.SetActive(false);
			Time.timeScale = 1;
		}
	}

	public void QuitToMenu()
	{
		PlayerPrefs.SetInt("quit to menu", 1);
		SceneManager.LoadScene("Menu");
	}

	public void Resume()
	{
		canvas.gameObject.SetActive(false);
		Time.timeScale = 1;
	}
}
