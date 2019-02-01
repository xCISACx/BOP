using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
	//Attach an Image you want to fade in the GameObject's Inspector
	public Image Wall;
	public Image BouncyInk;
	public Image SpeedyInk;
	public Image StickyInk;
	public TextMeshProUGUI Play;
	public TextMeshProUGUI Options;
	public TextMeshProUGUI Quit;
	
	//Use this to tell if the toggle returns true or false
	public static bool areInksFading;
	public static bool isWallFading;

	void Start()
	{
	}

	void Update()
	{	
		//If the toggle returns true, fade in the Image
		if (areInksFading)
		{
			//Fully fade in Image (1) with the duration of 1
			BouncyInk.CrossFadeAlpha(1, 1.0f, false);
			Play.CrossFadeAlpha(1, 1.0f, false);
			SpeedyInk.CrossFadeAlpha(1, 1.0f, false);
			Options.CrossFadeAlpha(1, 1.0f, false);
			StickyInk.CrossFadeAlpha(1, 1.0f, false);
			Quit.CrossFadeAlpha(1, 1.0f, false);
		}
		//If the toggle is false, fade out to nothing (0) the Image with a duration of 2
		if (!areInksFading)
		{
			BouncyInk.CrossFadeAlpha(0, 0.1f, false);
			Play.CrossFadeAlpha(0, 0.1f, false);
			SpeedyInk.CrossFadeAlpha(0, 0.1f, false);
			Options.CrossFadeAlpha(0, 0.1f, false);
			StickyInk.CrossFadeAlpha(0, 0.1f, false);
			Quit.CrossFadeAlpha(0, 0.1f, false);
		}
		
		if (isWallFading)
		{
			//Fully fade in Image (1) with the duration of 2
			Wall.CrossFadeAlpha(1, 0f, false);
		}
		//If the toggle is false, fade out to nothing (0) the Image with a duration of 2
		if (!isWallFading)
		{
			Wall.CrossFadeAlpha(0, 0f, false);
		}
		
	}

	/*public IEnumerator FadeIn()
	{
		isWallFading = true;
		areInksFading = true;
	}*/
}