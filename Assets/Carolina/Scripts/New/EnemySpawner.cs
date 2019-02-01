using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{

	public static EnemySpawner Instance;
	
	private bool _isCooldownTimerRunning = false;
	
	public float spawnCooldown;
	public Transform spawnpointsParent;
	private Transform[] spawnpoints;
	private int targetSpawnpointIndex = 0;
	private Transform targetSpawnpoint;
	public GameObject enemyPrefab;
	private Vector3 pos;
	public float timer = 0.0f;
	public SpawnInk.EnemyType enemySpawnType;

	// Use this for initialization
	void Start ()
	{
	
		Instance = this;

		spawnpoints = spawnpointsParent.GetComponentsInChildren<Transform>().Skip(1).ToArray();

		if (spawnpoints.Length == 0)
		{
			Debug.LogError("There are no spawn points!");
		}
		else Debug.Log("Found " + spawnpoints.Length + " spawn points.");	
	}
	
	// Update is called once per frame
	private void Update ()
	{
		if (!_isCooldownTimerRunning) return;
		
		timer += Time.deltaTime;
		if (timer >= spawnCooldown)
		{
			SpawnEnemy();
			_isCooldownTimerRunning = false;
			timer = 0;
		}
	}

	private void Awake()
	{
		SpawnEnemy();
	}

	public void SpawnEnemy()
	{
		var newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
		newEnemy.name = "Spawned Enemy " + targetSpawnpointIndex;
	}

	public void StartCooldownTimer()
	{
		_isCooldownTimerRunning = true;
	}
}
