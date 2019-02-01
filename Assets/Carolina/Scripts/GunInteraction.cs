using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GunInteraction : MonoBehaviour
{
	public GameObject gunStandInstance;
	public GameObject player;
	public Tilemap BedroomDoorTilemap;
	public GameObject UI;
	public SpawnInk spawnInk;
	public GameManager gameManager;
	public GameObject GunObtainedCanvas;
	public GameObject PlayerFirePoint;
	public PlayerBehaviour playerBehaviour;

	// Use this for initialization
	void Start ()
	{
		gameManager = FindObjectOfType<GameManager>();
		playerBehaviour = FindObjectOfType<PlayerBehaviour>();
		UI = GameObject.Find("UI Canvas");
		UI = GameObject.FindGameObjectWithTag("UI");

	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				playerBehaviour.uiActive = true;
				gameManager.playerHasGun = true; //setting the playerHasGun bool to true so that the player can shoot ink 
				GunObtainedCanvas.SetActive(true); //enabling the canvas that displays the text explaining how to use the gun
				PlayerFirePoint.GetComponent<SpriteRenderer>().enabled = true; //enabling the gun sprite attached to the player's hand
				BedroomDoorTilemap.ClearAllTiles();                           //opening the door for the player to go through                             //setting the UI bool to true
				spawnInk.playerHasUnlockedBouncy = true;
				playerBehaviour.uiActive = true; //setting the unlocked bouncy ammo bool to true (the player was only supposed to have bouncy and clear in the beginning)
				gunStandInstance.SetActive(false); //making the gun's sprite disappear from the gun stand
			}	
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (playerBehaviour.uiActive)
		{
			UI.SetActive(true);
		}		
	}
}
