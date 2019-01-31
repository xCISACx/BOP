using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{

	public PlayerStats PS;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {	
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("touched the pickup.");
		//gameObject.SetActive(false);
		PS.MaxBouncyAmmo++;
		Destroy(gameObject);
	}
}
