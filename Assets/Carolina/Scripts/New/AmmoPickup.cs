using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{

	public SpawnInk spawnInk;
	public PlayerBehaviour playerBehaviour;
	public AmmoBehaviour ammoBehaviour;

	// Use this for initialization
	void Start () 
	{
		playerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
		if (playerBehaviour.uiActive)
			spawnInk = GameObject.Find("InkSpray").GetComponent<SpawnInk>();
		ammoBehaviour = FindObjectOfType<AmmoBehaviour>();
	}
	
	// Update is called once per frame
	void Update () 
	{	
		playerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
		if (playerBehaviour.uiActive)
			spawnInk = GameObject.Find("InkSpray").GetComponent<SpawnInk>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//gameObject.SetActive(false);
		if (other.CompareTag("Bouncy Ammo"))
		{
			ammoBehaviour.currentBouncyAmmo += 5;
			Destroy(other.gameObject);
		}
		if(other.CompareTag("Speedy Ammo"))
		{
			ammoBehaviour.currentSpeedyAmmo += 5;
			Destroy(other.gameObject);
		}
		if(other.CompareTag("Sticky Ammo"))
		{
			ammoBehaviour.currentStickyAmmo += 5;
			Destroy(other.gameObject);
		}
		if(other.CompareTag("Clear Ammo"))
		{
			ammoBehaviour.currentClearAmmo += 5;
			Destroy(other.gameObject);
		}
	}
}
