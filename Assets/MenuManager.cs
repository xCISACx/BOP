using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MenuManager : MonoBehaviour
{

	public VideoPlayer videoPlayer;
	public FadeManager fadeManager;
	public GameObject Canvas;
	public GameObject Details;
	[SerializeField] public static bool videoPlayedOnce;
	
	// Use this for initialization
	void Start ()
	{

		fadeManager = FindObjectOfType<FadeManager>();
		videoPlayer = FindObjectOfType<VideoPlayer>();
		Canvas = FindObjectOfType<Canvas>().gameObject;

	}

	// Update is called once per frame
	void Update ()
	{
		if (videoPlayedOnce)
		{
			videoPlayer.playOnAwake = false;
			videoPlayer.gameObject.SetActive(false);
			StartCoroutine(FadeFaster());
		}
		
		if (videoPlayer.isPlaying)
		{
			Details.SetActive(true);
		}
		StartCoroutine(Fade());

		videoPlayer.loopPointReached += EndReached;
	}

	public void Play()
	{
		FadeManager.isWallFading = false;
		FadeManager.areInksFading = false;
		SceneManager.LoadScene("scene0");
	}

	public void Quit()
	{
		Application.Quit();
	}

	public IEnumerator Fade()
	{
		yield return new WaitForSeconds(6);
		FadeManager.isWallFading = true;
		FadeManager.areInksFading = true;
	}
	
	public IEnumerator FadeFaster()
	{
		FadeManager.isWallFading = true;
		Details.SetActive(true);
		yield return new WaitForSeconds(2);
		FadeManager.areInksFading = true;
	}
	
	void EndReached(VideoPlayer videoPlayer)
	{
		videoPlayedOnce = true;
	}
}
