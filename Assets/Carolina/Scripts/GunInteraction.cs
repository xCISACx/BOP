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
	public PlayerBehaviour PlayerBehaviour;
	public GameObject GunObtainedCanvas;
	public GameObject PlayerFirePoint;

	// Use this for initialization
	void Start () {
		
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Debug.Log("Collided with the player.");
			if (Input.GetKey(KeyCode.LeftControl))
			{
				PlayerBehaviour.playerHasGun = true;
				GunObtainedCanvas.SetActive(true);
				Debug.Log("Obtained the gun.");
				gunStandInstance.SetActive(false);
				PlayerFirePoint.GetComponent<SpriteRenderer>().enabled = true;
				BedroomDoorTilemap.ClearAllTiles();
				UI.SetActive(true);
				PlayerBehaviour.uiActive = true;
				spawnInk.playerHasUnlockedBouncy = true;
				Debug.Log("Unlocked the Bouncy ammo!");
			}	
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (PlayerBehaviour.uiActive)
		{
			UI.SetActive(true);
		}		
	}
}
