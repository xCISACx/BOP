using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
	
	public float BouncyAmmoPercent = 0;
	public float SpeedyAmmoPercent = 0;
	public float StickyAmmoPercent = 0;
	
	public float CurrentBouncyAmmo = 5;
	public int CurrentSpeedyAmmo = 5;
	public int CurrentStickyAmmo = 5;
	
	public int InitialBouncyAmmo = 5;
	public int InitialSpeedyAmmo = 5;
	public int InitialStickyAmmo = 5;
	
	public int MaxBouncyAmmo = 5;
	public int MaxSpeedyAmmo = 5;
	public int MaxStickyAmmo = 5;
	
	public Texture2D AmmoBottleBackground;
	public Texture2D BouncyAmmoBarTexture;
	
	public float BouncyAmmoBarLength;
	
	public bool playerHasUnlockedBouncy;
	public bool playerHasUnlockedSpeedy;
	public bool playerHasUnlockedSticky;
	
	public TextMeshProUGUI AmmoAmountText;
	public TextMeshProUGUI AmmoNameText;
	
	public GameObject UI;
	
	public bool uiActive;
	
	//public GameObject ColourIndicator;
	
	public Color32 BouncyColour;
	public Color32 SpeedyColour;
	public Color32 StickyColour;
	public Color32 ClearColour;
	
	public PhysicsMaterial2D BouncyMaterial;
	public PhysicsMaterial2D SpeedyMaterial;
	public PhysicsMaterial2D StickyMaterial;
	
	private ParticleSystem.MainModule settings;
	
	public Vector3 offsetY;
	
	public GameObject GroundCheck;

	// Use this for initialization
	void Start()
	{

		PrS = GetComponent<ParticleSystem>();
		collisionEvents = new List<ParticleCollisionEvent>();
		settings = PrS.main;
		settings.startColor = new ParticleSystem.MinMaxGradient(ClearColour);
		UI = GameObject.Find("Canvas");

	}

	// Update is called once per frame
	void Update()
	{
		InkPicking();
		AmmoCount();
	}

	void OnParticleCollision(GameObject other)
	{
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
					if (other.CompareTag("Ground") || other.gameObject.layer.ToString() == "Ground")
					{

						Vector3 pos = collisionEvents[i].intersection;
						GameObject newInkPuddle =
							Instantiate(inkPuddleStickyPrefab, pos + offsetY, Quaternion.identity);
						newInkPuddle.transform.up = collisionEvents[i].normal;
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
						if (enemyType != EnemyType.Clear)
						{
							enemyType = EnemyType.Clear;
						}
					}

					i++;
					break;
				}
			}
		}
	}

	void InkPicking()
	{
		if (Input.GetKeyDown(KeyCode.J))
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

		if (Input.GetKeyDown(KeyCode.K))
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

		if (Input.GetKeyDown(KeyCode.L))
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
		
		if (Input.GetKeyDown(KeyCode.P))
		{
			ammoType = AmmoType.Clear;
			Debug.Log("Ammo type: " + ammoType);
		}
		
		switch (ammoType)
		{
			case AmmoType.Bouncy:
				//AmmoNameText.color = BouncyColour;
				settings.startColor = new ParticleSystem.MinMaxGradient(BouncyColour);
				break;
			case AmmoType.Speedy:
				//AmmoNameText.color = SpeedyColour;
				settings.startColor = new ParticleSystem.MinMaxGradient(SpeedyColour);
				break;
			case AmmoType.Sticky:
				//AmmoNameText.color = StickyColour;
				settings.startColor = new ParticleSystem.MinMaxGradient(StickyColour);
				break;
			case AmmoType.Clear:
				//AmmoNameText.color = ClearColour;
				settings.startColor = new ParticleSystem.MinMaxGradient(ClearColour);
				break;
		}
	}

	void AmmoCount()
	{
		if (CurrentBouncyAmmo <= MaxBouncyAmmo)
		{
			BouncyAmmoPercent = CurrentBouncyAmmo/MaxBouncyAmmo;
			BouncyAmmoBarLength = (BouncyAmmoPercent * 50);
			//AmmoAmountText.text = "" + CurrentBouncyAmmo + " / " + MaxBouncyAmmo;
		}
		
		if (CurrentBouncyAmmo > MaxBouncyAmmo)
		{
			CurrentBouncyAmmo = MaxBouncyAmmo;
		}

		if (CurrentBouncyAmmo < 0)
		{
			CurrentBouncyAmmo = 0;
		}
	}

	private void OnGUI()
	{
		if (UI.activeSelf)
		{
			//guiStyle.fontSize = 50;
			GUI.DrawTexture(new Rect(450,-10,80,110), AmmoBottleBackground);
			GUI.DrawTexture(new Rect(475,75,30,-BouncyAmmoBarLength), BouncyAmmoBarTexture);
			//GUI.Label(new Rect(550,20,100000,100000), "" + CurrentBouncyAmmo + " / " + MaxBouncyAmmo, guiStyle);
		}
	}
}
	
	
