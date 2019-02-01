using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAmmo : MonoBehaviour {
	
	public EnemyStats enemyStats;
	public PlayerBehaviour playerBehaviour;
	public GameObject bouncyAmmoBottlePrefab;
	public GameObject speedyAmmoBottlePrefab;
	public GameObject stickyAmmoBottlePrefab;
	public GameObject clearAmmoBottlePrefab;
	public Vector3 offsetY;

	// Use this for initialization
	void Start ()
	{
		playerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
		//if (playerBehaviour.uiActive)
			enemyStats = GetComponent<EnemyStats>();

	}
	
	// Update is called once per frame
	void Update () {
		
		playerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
		
		
	}

	public void TypeCheck()
	{
		switch (enemyStats.type)
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
