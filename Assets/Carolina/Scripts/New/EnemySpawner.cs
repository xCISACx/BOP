using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
	
	private bool _isCooldownTimerRunning = false;
	
	public float spawnCooldown;
	private Transform targetSpawnpoint;
	public GameObject enemyPrefab;
	private Vector3 pos;
	public float timer = 0.0f;
	public SpawnInk.EnemyType enemySpawnType;

	// Use this for initialization
	void Start ()
	{

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
		newEnemy.name = "Spawned Enemy ";
		newEnemy.GetComponent<EnemyStats>().spawner = this;
		newEnemy.GetComponent<EnemyStats>().type = enemySpawnType;
	}

	public void StartCooldownTimer()
	{
		_isCooldownTimerRunning = true;
	}
}
