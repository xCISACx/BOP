using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAmmo : MonoBehaviour {
	
	public SpawnInk spawnInk;
	public GameObject bouncyAmmoBottlePrefab;
	public GameObject speedyAmmoBottlePrefab;
	public GameObject stickyAmmoBottlePrefab;
	public GameObject clearAmmoBottlePrefab;
	public Vector3 offsetY;

	// Use this for initialization
	void Start ()
	{

		spawnInk = GameObject.Find("InkSpray").GetComponent<SpawnInk>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TypeCheck()
	{
		switch (spawnInk.enemyType)
		{
			case SpawnInk.EnemyType.Bouncy:
				Instantiate(bouncyAmmoBottlePrefab, transform.position + offsetY, Quaternion.identity);
				break;
			case SpawnInk.EnemyType.Speedy:
				Instantiate(speedyAmmoBottlePrefab, transform.position + offsetY, Quaternion.identity);
				break;
			case SpawnInk.EnemyType.Sticky:
				Instantiate(stickyAmmoBottlePrefab, transform.position + offsetY, Quaternion.identity);
				break;
			case SpawnInk.EnemyType.Clear:
				Instantiate(clearAmmoBottlePrefab, transform.position + offsetY, Quaternion.identity);
				break;
		}
	}
}
