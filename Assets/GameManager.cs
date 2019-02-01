using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public Transform playerSpawnPoint;
	public GameObject player;
	public GameObject playerPrefab;
	public GameManager gameManager;
	GameObject newPlayer;
	private CapsuleCollider2D playerCollider;
	PlayerBehaviour playerBehaviour;

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
}
