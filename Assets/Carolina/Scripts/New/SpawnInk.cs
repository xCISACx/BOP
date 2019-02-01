using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class SpawnInk : MonoBehaviour
{
	public enum AmmoType
	{
		Clear,
		Bouncy,
		Speedy,
		Sticky,
	}

	public enum EnemyType
	{
		Clear,
		Bouncy,
		Speedy,
		Sticky,
	}

	public GameObject inkPuddleBouncyPrefab;
	public GameObject inkPuddleSpeedyPrefab;
	public GameObject inkPuddleStickyPrefab;
	public GameObject inkPuddleClearPrefab;
	
	public ParticleSystem PrS;
	
	public List<ParticleCollisionEvent> collisionEvents;
	
	public AmmoType ammoType;
	
	public bool playerHasUnlockedBouncy;
	public bool playerHasUnlockedSpeedy;
	public bool playerHasUnlockedSticky;
	
	public Color32 BouncyColour;
	public Color32 SpeedyColour;
	public Color32 StickyColour;
	public Color32 ClearColour;
	
	public PhysicsMaterial2D BouncyMaterial;
	public PhysicsMaterial2D SpeedyMaterial;
	public PhysicsMaterial2D StickyMaterial;
	
	public ParticleSystem.MainModule settings;
	
	public Vector3 offsetY;
	public Vector3 wallOffset;
	
	public GameManager GM;
	public PlayerBehaviour playerBehaviour;
	public AmmoBehaviour ammoBehaviour;

	// Use this for initialization
	void Start()
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
		playerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
		PrS = GameObject.Find("InkSpray").GetComponent<ParticleSystem>();
		collisionEvents = new List<ParticleCollisionEvent>();
		settings = PrS.main;
		settings.startColor = new ParticleSystem.MinMaxGradient(ClearColour);
		ammoBehaviour = FindObjectOfType<AmmoBehaviour>();

	}

	// Update is called once per frame
	void Update()
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
		PrS = GameObject.Find("InkSpray").GetComponent<ParticleSystem>();
		playerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
		collisionEvents = new List<ParticleCollisionEvent>();
		settings = PrS.main;
		settings.startColor = new ParticleSystem.MinMaxGradient(ClearColour);
		InkPicking();
		AmmoCount();
	}

	void OnParticleCollision(GameObject other)
	{
		ammoBehaviour.SpendAmmo(1);
		//Debug.Log("OnParticleCollision -> " + other);
		
		if (other.CompareTag("Acid") || other.gameObject.layer.ToString() == "Acid") return;

		int i = 0;
		int numCollisionEvents = PrS.GetCollisionEvents(other, collisionEvents);
		while (i < numCollisionEvents)
		{
			switch (ammoType)
			{
				case AmmoType.Bouncy:
				{
					if (other.CompareTag("Ground") || other.gameObject.layer.ToString() == "Ground")   //if the bouncy ink is selected and the particles collide with the ground, instantiate a puddle of the correct type
					{
						Vector3 pos = collisionEvents[i].intersection;
						GameObject newInkPuddle =
							Instantiate(inkPuddleBouncyPrefab, pos + offsetY, Quaternion.identity);
						newInkPuddle.transform.up = collisionEvents[i].normal; //transform the instantiated puddle so that it matches the surface it spawns on TODO:Fix the diagonal ink spawning.
					}

					if (other.CompareTag("Enemy") || other.gameObject.layer.ToString() == "Enemy") //if the particles hit an enemy...
					{
						if (ammoBehaviour.currentBouncyAmmo == 0)
							return; //do nothing if there's no ammo TODO: NOT WORKING, prevent ink from being spawned when there's no ammo.
						
						var enemyStats = other.GetComponent<EnemyStats>(); //get the Enemy Stats component of the specific enemy that we hit
						enemyStats.ApplyDamage(1); //reduce the enemy's HP by 1
						
						
						if (enemyStats.type == EnemyType.Bouncy) //if the enemy hit is of the bouncy type...
						{
							Debug.Log("Enemy is already " + enemyStats.type); 
							return; //do nothing because it doesn't need to change type
						}

						
						if (enemyStats.type == EnemyType.Speedy) //if the enemy hit is of the speedy type...
						{
							enemyStats.type = EnemyType.Sticky; //change its type to sticky
						}

						if (enemyStats.type == EnemyType.Sticky) //if the enemy hit is of the sticky type...
						{
							return; //do nothing because sticky can only be cleared with clear ink
						}
						
						if (enemyStats.type == EnemyType.Clear) //if the enemy hit is of the clear type...
						{
							enemyStats.type = EnemyType.Bouncy; //change its type to bouncy
							return;
						}
					}
					
					if (other.CompareTag("Puddle") || other.gameObject.layer.ToString() == "Puddle") return;

					i++;
					break;
				}
				case (AmmoType.Speedy):
				{
					if (ammoBehaviour.currentSpeedyAmmo == 0)
						return;
					
					if (other.CompareTag("Ground") || other.gameObject.layer.ToString() == "Ground") //if the speedy ink is selected and the particles collide with the ground, instantiate a puddle of the correct type
					{

						Vector3 pos = collisionEvents[i].intersection;
						GameObject newInkPuddle =
							Instantiate(inkPuddleSpeedyPrefab, pos + offsetY, Quaternion.identity);
						newInkPuddle.transform.up = collisionEvents[i].normal;
					}

					if (other.CompareTag("Enemy") || other.gameObject.layer.ToString() == "Enemy") //if the particles hit an enemy...
					{
						var enemyStats = other.GetComponent<EnemyStats>();
						enemyStats.ApplyDamage(1);
						
						if (enemyStats.type == EnemyType.Sticky) //if the enemy hit is of the sticky type...
						{
							Debug.Log("Enemy is already " + enemyStats.type + ", can't change it back.");
							return; //do nothing, sticky can only be cleared with clear ink
						}
						if (enemyStats.type == EnemyType.Speedy) //if the enemy hit is of the speedy type...
						{
							Debug.Log("Enemy is already " + enemyStats.type);
							return; //do nothing, it already is the correct type
						}
						Debug.Log(enemyStats.type);
						
						if (enemyStats.type == EnemyType.Bouncy) //if the enemy hit is of the speedy type...
						{
							enemyStats.type = EnemyType.Sticky; //change its type to sticky
						}
						else enemyStats.type = EnemyType.Speedy; //if not, change it to speedy
						
						
						if (enemyStats.type == EnemyType.Clear) //if the enemy hit is of the clear type...
						{
							enemyStats.type = EnemyType.Speedy; //change its type to speedy
						}
					}
					
					if (other.CompareTag("Puddle") || other.gameObject.layer.ToString() == "Puddle") return;

					i++;
					break;
				}
				case (AmmoType.Sticky):
				{
					if (ammoBehaviour.currentStickyAmmo == 0)
						return;
					
					if (other.CompareTag("Ground") || other.gameObject.layer.ToString() == "Ground") //if the sticky ink is selected and the particles collide with the ground, instantiate a puddle of the correct type
					{

						Vector3 pos = collisionEvents[i].intersection;
						GameObject newInkPuddle =
							Instantiate(inkPuddleStickyPrefab, pos + offsetY, Quaternion.identity);
						newInkPuddle.transform.up = collisionEvents[i].normal;
					}

					if (other.CompareTag("Walls") || other.gameObject.layer.ToString() == "Walls")
					{
						Vector3 pos = collisionEvents[i].intersection;
						GameObject newInkPuddle =
							Instantiate(inkPuddleStickyPrefab, pos + wallOffset, Quaternion.identity);
						newInkPuddle.transform.up = collisionEvents[i].normal;
					}

					if (other.CompareTag("Enemy") || other.gameObject.layer.ToString() == "Enemy")
					{
						var enemyStats = other.GetComponent<EnemyStats>();
						enemyStats.ApplyDamage(1);

						enemyStats.type = EnemyType.Sticky; //sticky overwrites every other type


						if (enemyStats.type == EnemyType.Clear) //if the enemy hit is of the clear type...
						{
							enemyStats.type = EnemyType.Sticky; //change its type to sticky
							return;
						}
					}
					
					if (other.CompareTag("Puddle") || other.gameObject.layer.ToString() == "Puddle") return; //do not instantiate a puddle where there already is one TODO: NOT WORKING, fix this to improve performance.

					i++;
					break;
				}
				case (AmmoType.Clear):
				{										
					if (other.gameObject.CompareTag("Puddle") || other.gameObject.layer.ToString() == "Puddle" || other.gameObject.layer.ToString() == "Sticky Puddle")  //if we hit a puddle with the clear ink...
					{
						Destroy(other.gameObject); //destroy the puddle
					}

					if (other.CompareTag("Enemy") || other.gameObject.layer.ToString() == "Enemy") //if the particles hit an enemy...
					{
						var enemyStats = other.GetComponent<EnemyStats>();
						
						enemyStats.ApplyDamage(1); //reduce the enemy's HP by 1
						Debug.Log(enemyStats.type);
						enemyStats.type = EnemyType.Clear; //change its type to clear
					}

					i++;
					break;
				}
			}
		}
	}

	void InkPicking()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if (playerHasUnlockedBouncy)
			{
				ammoType = AmmoType.Bouncy;
				Debug.Log("Ammo type: " + ammoType);
			}
			else if (!playerHasUnlockedBouncy)
			{
				Debug.Log("Player hasn't unlocked Bouncy ammo yet.");
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			if (playerHasUnlockedSpeedy)
			{
				ammoType = AmmoType.Speedy;
				Debug.Log("Ammo type: " + ammoType);
			}
			else if (!playerHasUnlockedSpeedy)
			{
				Debug.Log("Player hasn't unlocked Speedy ammo yet.");
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			if (playerHasUnlockedSticky)
			{
				ammoType = AmmoType.Sticky;
				Debug.Log("Ammo type: " + ammoType);
			}
			else if (!playerHasUnlockedSticky)
			{
				Debug.Log("Player hasn't unlocked Sticky ammo yet.");
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			ammoType = AmmoType.Clear;
			Debug.Log("Ammo type: " + ammoType);
		}
		
		switch (ammoType) //changes the colour of the particle system depending on the ammo type that is selected TODO: Not working correctly, colour isn't being set as the given colour,
																												//TODO: possibly due to the particle system being cyan already.
		{
			case AmmoType.Bouncy:
				settings.startColor = new ParticleSystem.MinMaxGradient(BouncyColour);
				break;
			case AmmoType.Speedy:
				settings.startColor = new ParticleSystem.MinMaxGradient(SpeedyColour);
				break;
			case AmmoType.Sticky:
				settings.startColor = new ParticleSystem.MinMaxGradient(StickyColour);
				break;
			case AmmoType.Clear:
				settings.startColor = new ParticleSystem.MinMaxGradient(ClearColour);
				break;
		}
	}

	void AmmoCount() //TODO: Make it work for all the ammo types.
	{
		if (ammoBehaviour.currentBouncyAmmo <= ammoBehaviour.maxBouncyAmmo) //if the current bouncy ammo is lesser than/equal to the max bouncy ammo...
		{
			ammoBehaviour.bouncyAmmoPercent = ammoBehaviour.currentBouncyAmmo/ammoBehaviour.maxBouncyAmmo; //the percentage of bouncy ammo will be the current bouncy ammo divided by the max ammo.
		}
	}
}
	
	
