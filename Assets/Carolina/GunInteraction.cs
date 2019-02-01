using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GunInteraction : MonoBehaviour
{

	public FireBehaviour FB;
	public GameObject gunStandInstance;
	public GameObject player;
	public Tilemap BedroomDoorTilemap;
	public GameObject UI;
	public InkBehaviour IB;
	public PlayerStats PS;
	public GameObject GunObtainedCanvas;
	public GameObject PlayerFirePoint;

	// Use this for initialization
	void Start () {
		
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			Debug.Log("Collided with the player.");
			if (Input.GetKey(KeyCode.Z))
			{
				FB.playerHasGun = true;
				GunObtainedCanvas.SetActive(true);
				Debug.Log("Obtained the gun.");
				gunStandInstance.SetActive(false);
				PlayerFirePoint.SetActive(true);
				BedroomDoorTilemap.ClearAllTiles();
				UI.SetActive(true);
				PS.UIisActive = true;
				PS.playerHasUnlockedBouncy = true;
				Debug.Log("Unlocked the Bouncy ammo!");
			}	
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (PS.UIisActive)
		{
			UI.SetActive(true);
		}
		
	}
}
