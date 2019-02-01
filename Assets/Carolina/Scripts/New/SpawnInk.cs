using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

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
	
	public ParticleSystem PrS;
	
	public List<ParticleCollisionEvent> collisionEvents;
	
	public AmmoType ammoType;
	public EnemyType enemyType;
	
	public Texture2D AmmoBottleBackground;
	public Texture2D BouncyAmmoBarTexture;
	
	public float BouncyAmmoBarLength;
	
	public bool playerHasUnlockedBouncy;
	public bool playerHasUnlockedSpeedy;
	public bool playerHasUnlockedSticky;
	
	//public GameObject ColourIndicator;
	
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
	
	public GameObject GroundCheck;
	public GameManager GM;
	public PlayerBehaviour playerBehaviour;

	// Use this for initialization
	void Start()
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
		playerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
		PrS = GameObject.Find("InkSpray").GetComponent<ParticleSystem>();
		collisionEvents = new List<ParticleCollisionEvent>();
		settings = PrS.main;
		settings.startColor = new ParticleSystem.MinMaxGradient(ClearColour);

	}

	// Update is called once per frame
	void Update()
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
		PrS = GameObject.Find("InkSpray").GetComponent<ParticleSystem>();
		collisionEvents = new List<ParticleCollisionEvent>();
		settings = PrS.main;
		settings.startColor = new ParticleSystem.MinMaxGradient(ClearColour);
		InkPicking();
		AmmoCount();
	}

	void OnParticleCollision(GameObject other)
	{
		playerBehaviour.SpendAmmo(1);
		Debug.Log("OnParticleCollision -> " + other);
		
		if (other.CompareTag("Puddle") || other.gameObject.layer.ToString() == "Puddle") return;
		if (other.CompareTag("Acid") || other.gameObject.layer.ToString() == "Acid") return;

		int i = 0;
		int numCollisionEvents = PrS.GetCollisionEvents(other, collisionEvents);
		while (i < numCollisionEvents)
		{
			switch (ammoType)
			{
				case AmmoType.Bouncy:
				{
					if (playerBehaviour.currentBouncyAmmo == 0)
						return;
					if (other.CompareTag("Ground") || other.gameObject.layer.ToString() == "Ground")
					{

						Vector3 pos = collisionEvents[i].intersection;
						GameObject newInkPuddle =
							Instantiate(inkPuddleBouncyPrefab, pos + offsetY, Quaternion.identity);
						newInkPuddle.transform.up = collisionEvents[i].normal;
					}

					if (other.CompareTag("Enemy") || other.gameObject.layer.ToString() == "Enemy")
					{
						var enemyStats = other.GetComponent<EnemyStats>();
						
						enemyStats.ApplyDamage(1);
						other.gameObject.GetComponent<SpriteRenderer>().color = BouncyColour;
						
						
						if (enemyType == EnemyType.Bouncy)
						{
							Debug.Log("Enemy is already " + enemyType);
							return;
						}

						
						if (enemyType == EnemyType.Speedy)
						{
							enemyType = EnemyType.Sticky;
							other.gameObject.GetComponent<SpriteRenderer>().color = StickyColour;
						}

						if (enemyType == EnemyType.Sticky)
						{
							return;
						}
						
						if (enemyType == EnemyType.Clear)
						{
							enemyType = EnemyType.Bouncy;
							return;
						}
					}

					i++;
					break;
				}
				case (AmmoType.Speedy):
				{
					if (playerBehaviour.currentSpeedyAmmo == 0)
						return;
					if (other.CompareTag("Ground") || other.gameObject.layer.ToString() == "Ground")
					{

						Vector3 pos = collisionEvents[i].intersection;
						GameObject newInkPuddle =
							Instantiate(inkPuddleSpeedyPrefab, pos + offsetY, Quaternion.identity);
						newInkPuddle.transform.up = collisionEvents[i].normal;
					}

					if (other.CompareTag("Enemy") || other.gameObject.layer.ToString() == "Enemy")
					{
						var enemyStats = other.GetComponent<EnemyStats>();
						
						enemyStats.ApplyDamage(1);
						
						if (enemyType == EnemyType.Sticky)
						{
							Debug.Log("Enemy is already " + enemyType + ", can't change it back.");
							return;
						}
						if (enemyType == EnemyType.Speedy)
						{
							Debug.Log("Enemy is already " + enemyType);
							return;
						}

						Debug.Log(enemyType);
						
						other.gameObject.GetComponent<SpriteRenderer>().color = SpeedyColour;
						if (enemyType == EnemyType.Bouncy)
						{
							enemyType = EnemyType.Sticky;
							other.gameObject.GetComponent<SpriteRenderer>().color = StickyColour;
						}
						else enemyType = EnemyType.Speedy;
						
						if (enemyType == EnemyType.Clear)
						{
							enemyType = EnemyType.Speedy;
							return;
						}
					}

					i++;
					break;
				}
				case (AmmoType.Sticky):
				{
					if (playerBehaviour.currentStickyAmmo == 0)
						return;
					if (other.CompareTag("Ground") || other.gameObject.layer.ToString() == "Ground")
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
						other.GetComponent<CompositeCollider2D>().sharedMaterial = StickyMaterial;
						Debug.Log("Changing the wall material to sticky");
					}

					if (other.CompareTag("Enemy") || other.gameObject.layer.ToString() == "Enemy")
					{
						var enemyStats = other.GetComponent<EnemyStats>();
						
						enemyStats.ApplyDamage(1);

						other.gameObject.GetComponent<SpriteRenderer>().color = StickyColour;
						enemyType = EnemyType.Sticky;
					}
					
					if (enemyType == EnemyType.Clear)
					{
						enemyType = EnemyType.Sticky;
						return;
					}

					i++;
					break;
				}
				case (AmmoType.Clear):
				{
					if (playerBehaviour.currentClearAmmo == 0)
						return;
					if (other.CompareTag("Puddle") || other.gameObject.layer.ToString() == "Puddle")
					{

						Vector3 pos = collisionEvents[i].intersection;
						Destroy(other);
					}

					if (other.CompareTag("Enemy") || other.gameObject.layer.ToString() == "Enemy")
					{
						var enemyStats = other.GetComponent<EnemyStats>();
						
						enemyStats.ApplyDamage(1);
						
						Debug.Log(enemyType);
						other.gameObject.GetComponent<SpriteRenderer>().color = ClearColour;
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
		
		switch (ammoType)
		{
			case AmmoType.Bouncy:
				//GM.AmmoLevelImage.color = BouncyColour;
				//AmmoNameText.color = BouncyColour;
				settings.startColor = new ParticleSystem.MinMaxGradient(BouncyColour);
				break;
			case AmmoType.Speedy:
				//AmmoNameText.color = SpeedyColour;
				settings.startColor = new ParticleSystem.MinMaxGradient(SpeedyColour);
				//GM.AmmoLevelImage.color = SpeedyColour;
				break;
			case AmmoType.Sticky:
				//AmmoNameText.color = StickyColour;
				settings.startColor = new ParticleSystem.MinMaxGradient(StickyColour);
				//GM.AmmoLevelImage.color = StickyColour;
				break;
			case AmmoType.Clear:
				//AmmoNameText.color = ClearColour;
				settings.startColor = new ParticleSystem.MinMaxGradient(ClearColour);
				//GM.AmmoLevelImage.color = ClearColour;
				break;
		}
	}

	void AmmoCount()
	{
		if (playerBehaviour.currentBouncyAmmo <= playerBehaviour.maxBouncyAmmo)
		{
			playerBehaviour.bouncyAmmoPercent = playerBehaviour.currentBouncyAmmo/playerBehaviour.maxBouncyAmmo;
			//BouncyAmmoBarLength = (BouncyAmmoPercent * 50);
			//AmmoAmountText.text = "" + CurrentBouncyAmmo + " / " + MaxBouncyAmmo;
		}
	}
}
	
	
