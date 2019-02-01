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
	GameObject newPlayer;
	private CapsuleCollider2D playerCollider;
	PlayerBehaviour playerBehaviour;
	Vector3 playerScale;
	public Image BouncyAmmoLevelImage;
	public Image SpeedyAmmoLevelImage;
	public Image StickyAmmoLevelImage;
	public Image ClearAmmoLevelImage;
	public GameObject bouncyAmmoBar;
	public GameObject speedyAmmoBar;
	public GameObject stickyAmmoBar;
	public GameObject clearAmmoBar;

	// Use this for initialization
	void Start () 
	{
		if (gameManager == null)
		{
			gameManager = this;
		}
		
		player = GameObject.FindGameObjectWithTag("Player");
		playerPrefab = GameObject.FindGameObjectWithTag("Player");
		playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider2D>();
		playerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerPrefab = GameObject.FindGameObjectWithTag("Player");
		playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider2D>();
		playerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
		ReduceAmmoBar();
	}

	public void RespawnPlayer()
	{
		Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
		playerCollider.enabled = true;
		playerBehaviour.enabled = true;
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
			bouncyAmmoBar = GameObject.Find("Bouncy Ammo Level Image");
			speedyAmmoBar = GameObject.Find("Speedy Ammo Level Image");
			stickyAmmoBar = GameObject.Find("Sticky Ammo Level Image");
			clearAmmoBar = GameObject.Find("Clear Ammo Level Image");
			
			//BOUNCY
			if (bouncyAmmoBar == null || BouncyAmmoLevelImage == null || BouncyAmmoLevelImage.rectTransform == null ||
			    BouncyAmmoLevelImage.rectTransform.sizeDelta == null)
				return;
			var bouncyAmmoBarRectTransform = bouncyAmmoBar.transform as RectTransform;
			var bouncyAmmoLevelImageLength = BouncyAmmoLevelImage.rectTransform.sizeDelta.x;
			bouncyAmmoLevelImageLength = playerBehaviour.currentBouncyAmmo * 20;
			if (bouncyAmmoBarRectTransform != null)
				bouncyAmmoBarRectTransform.sizeDelta = new Vector2(bouncyAmmoLevelImageLength, bouncyAmmoBarRectTransform.sizeDelta.y);
			if (bouncyAmmoBarRectTransform.sizeDelta.x < 0)
				bouncyAmmoBarRectTransform.sizeDelta = new Vector2(0, bouncyAmmoBarRectTransform.sizeDelta.y);
			
			//SPEEDY
			var speedyAmmoBarRectTransform = speedyAmmoBar.transform as RectTransform;
			var speedyAmmoLevelImageLength = SpeedyAmmoLevelImage.rectTransform.sizeDelta.x;
			speedyAmmoLevelImageLength = playerBehaviour.currentSpeedyAmmo * 20;
			if (speedyAmmoBarRectTransform != null)
				speedyAmmoBarRectTransform.sizeDelta = new Vector2(speedyAmmoLevelImageLength, speedyAmmoBarRectTransform.sizeDelta.y);
			if (speedyAmmoBarRectTransform.sizeDelta.x < 0)
				speedyAmmoBarRectTransform.sizeDelta = new Vector2(0, speedyAmmoBarRectTransform.sizeDelta.y);
			
			//STICKY
			var stickyAmmoBarRectTransform = stickyAmmoBar.transform as RectTransform;
			var stickyAmmoLevelImageLength = StickyAmmoLevelImage.rectTransform.sizeDelta.x;
			stickyAmmoLevelImageLength = playerBehaviour.currentStickyAmmo * 20;
			if (stickyAmmoBarRectTransform != null)
				stickyAmmoBarRectTransform.sizeDelta = new Vector2(stickyAmmoLevelImageLength, stickyAmmoBarRectTransform.sizeDelta.y);
			if (stickyAmmoBarRectTransform.sizeDelta.x < 0)
				stickyAmmoBarRectTransform.sizeDelta = new Vector2(0, stickyAmmoBarRectTransform.sizeDelta.y);
			
			//CLEAR
			var clearAmmoBarRectTransform = clearAmmoBar.transform as RectTransform;
			var clearAmmoLevelImageLength = ClearAmmoLevelImage.rectTransform.sizeDelta.x;
			clearAmmoLevelImageLength = playerBehaviour.currentClearAmmo * 20;
			if (stickyAmmoBarRectTransform != null)
				clearAmmoBarRectTransform.sizeDelta = new Vector2(clearAmmoLevelImageLength, clearAmmoBarRectTransform.sizeDelta.y);
			if (clearAmmoBarRectTransform.sizeDelta.x < 0)
				clearAmmoBarRectTransform.sizeDelta = new Vector2(0, clearAmmoBarRectTransform.sizeDelta.y);
		}
	}
}