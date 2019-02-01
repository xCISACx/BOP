using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	public Transform playerSpawnPoint;
	public GameObject player;
	public GameObject playerPrefab;
	public GameManager gameManager;
	public AmmoBehaviour ammoBehaviour;
	GameObject newPlayer;
	private CapsuleCollider2D playerCollider;
	PlayerBehaviour playerBehaviour;
	Vector3 playerScale;
	public Vector3 defaultScale;
	public Image BouncyAmmoLevelImage;
	public Image SpeedyAmmoLevelImage;
	public Image StickyAmmoLevelImage;
	public Image ClearAmmoLevelImage;
	public GameObject bouncyAmmoBar;
	public GameObject speedyAmmoBar;
	public GameObject stickyAmmoBar;
	public GameObject clearAmmoBar;
	
	public bool playerHasGun;
	public bool isGameStart;

	// Use this for initialization
	void Start()
	{
		if (gameManager == null)
		{
			gameManager = this;
		}

		playerScale = player.transform.localScale;
		Debug.Log(playerScale);
		player = GameObject.FindGameObjectWithTag("Player");
		playerPrefab = GameObject.FindGameObjectWithTag("Player");
		playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider2D>();
		playerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
		ammoBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<AmmoBehaviour>();
	}

	// Update is called once per frame
	void Update () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerPrefab = GameObject.FindGameObjectWithTag("Player");
		playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider2D>();
		playerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
		ammoBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<AmmoBehaviour>();
		ReduceAmmoBar();
	}

	public void RespawnPlayer()
	{
		playerPrefab.transform.localScale = defaultScale; //we set the player's scale to a preset scale so that the player won't spawn facing the opposite direction.
		Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation); //instantiating the player prefab and enabling the collider and behaviour script since they are disabled by default.
		playerCollider.enabled = true;
		playerBehaviour.enabled = true;
		isGameStart = false;
		playerHasGun = true;
	}

	public void KillPlayer()
	{
		gameManager.RespawnPlayer();
		Destroy(player);
	}

	public void ReduceAmmoBar()
	{
		if (playerBehaviour.uiActive)
		{
			//DEFINING ALL THE IMAGES OF THE UI THAT REPRESENT THE LEVELS OF INK
			/*bouncyAmmoBar = GameObject.Find("Bouncy Ammo Level Image");
			speedyAmmoBar = GameObject.Find("Speedy Ammo Level Image");
			stickyAmmoBar = GameObject.Find("Sticky Ammo Level Image");
			clearAmmoBar = GameObject.Find("Clear Ammo Level Image");*/
			
			bouncyAmmoBar = GameObject.FindGameObjectWithTag("Bouncy Meter");
			speedyAmmoBar = GameObject.FindGameObjectWithTag("Speedy Meter");
			stickyAmmoBar = GameObject.FindGameObjectWithTag("Sticky Meter");
			clearAmmoBar = GameObject.FindGameObjectWithTag("Clear Meter");
			
			//BOUNCY
			if (bouncyAmmoBar == null || BouncyAmmoLevelImage == null || BouncyAmmoLevelImage.rectTransform == null ||
			    BouncyAmmoLevelImage.rectTransform.sizeDelta == null)
				return;
			
			var bouncyAmmoBarRectTransform = bouncyAmmoBar.transform as RectTransform;
			var bouncyAmmoLevelImageLength = BouncyAmmoLevelImage.rectTransform.sizeDelta.x;
			bouncyAmmoLevelImageLength = ammoBehaviour.currentBouncyAmmo * 20;
			if (bouncyAmmoBarRectTransform != null)
				bouncyAmmoBarRectTransform.sizeDelta = new Vector2(bouncyAmmoLevelImageLength, bouncyAmmoBarRectTransform.sizeDelta.y);
			if (bouncyAmmoBarRectTransform.sizeDelta.x < 0)
				bouncyAmmoBarRectTransform.sizeDelta = new Vector2(0, bouncyAmmoBarRectTransform.sizeDelta.y);
			
			//SPEEDY
			var speedyAmmoBarRectTransform = speedyAmmoBar.transform as RectTransform;
			var speedyAmmoLevelImageLength = SpeedyAmmoLevelImage.rectTransform.sizeDelta.x;
			speedyAmmoLevelImageLength = ammoBehaviour.currentSpeedyAmmo * 20;
			if (speedyAmmoBarRectTransform != null)
				speedyAmmoBarRectTransform.sizeDelta = new Vector2(speedyAmmoLevelImageLength, speedyAmmoBarRectTransform.sizeDelta.y);
			if (speedyAmmoBarRectTransform.sizeDelta.x < 0)
				speedyAmmoBarRectTransform.sizeDelta = new Vector2(0, speedyAmmoBarRectTransform.sizeDelta.y);
			
			//STICKY
			var stickyAmmoBarRectTransform = stickyAmmoBar.transform as RectTransform;
			var stickyAmmoLevelImageLength = StickyAmmoLevelImage.rectTransform.sizeDelta.x;
			stickyAmmoLevelImageLength = ammoBehaviour.currentStickyAmmo * 20;
			if (stickyAmmoBarRectTransform != null)
				stickyAmmoBarRectTransform.sizeDelta = new Vector2(stickyAmmoLevelImageLength, stickyAmmoBarRectTransform.sizeDelta.y);
			if (stickyAmmoBarRectTransform.sizeDelta.x < 0)
				stickyAmmoBarRectTransform.sizeDelta = new Vector2(0, stickyAmmoBarRectTransform.sizeDelta.y);
			
			//CLEAR
			var clearAmmoBarRectTransform = clearAmmoBar.transform as RectTransform;
			var clearAmmoLevelImageLength = ClearAmmoLevelImage.rectTransform.sizeDelta.x;
			clearAmmoLevelImageLength = ammoBehaviour.currentClearAmmo * 20;
			if (stickyAmmoBarRectTransform != null)
				clearAmmoBarRectTransform.sizeDelta = new Vector2(clearAmmoLevelImageLength, clearAmmoBarRectTransform.sizeDelta.y);
			if (clearAmmoBarRectTransform.sizeDelta.x < 0)
				clearAmmoBarRectTransform.sizeDelta = new Vector2(0, clearAmmoBarRectTransform.sizeDelta.y);
		}
	}
}