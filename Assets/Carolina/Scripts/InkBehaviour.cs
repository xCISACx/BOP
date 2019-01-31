using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class InkBehaviour : MonoBehaviour
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

	public PhysicsMaterial2D BouncyMaterial;
	public PhysicsMaterial2D SpeedyMaterial;
	public PhysicsMaterial2D StickyMaterial;
	public GameObject ColourIndicator;
	public AmmoType ammoType;
	public EnemyType enemyType;
	public SimplePlayerController SPC;
	public FireBehaviour FB;
	public PlayerStats PS;
	public TextMeshProUGUI AmmoNameText;
	public Tile HitTile;
	public Tilemap PlatformsTilemap;
	public GameObject inkPuddlePrefab;
	private TileBase tileToChange;
	private Vector2 _contactPoint;
	public Grid PlatformsGrid;
	
	
	//private int _numberOfAmmoTypes = System.Enum.GetValues(typeof(AmmoType)).Length;

	private void Awake()
	{
		
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		AmmoNameText.text = ammoType.ToString();
		
		/*var ammo_type = (int) ammoType;
		
		if (ammo_type >= _numberOfAmmoTypes)
		{
			ammo_type = 1;
		}
		
		ammoType = (AmmoType) ammo_type;
		
		if(Input.GetKeyDown(KeyCode.L))
		{
			ammoType += 1;
			Debug.Log("Ammo type: " + ammoType);
		}*/


		if (Input.GetKeyDown(KeyCode.J))
		{
			if (PS.playerHasUnlockedBouncy)
			{
				ammoType = AmmoType.Bouncy;
				Debug.Log("Ammo type: " + ammoType);
			}
			else if (!PS.playerHasUnlockedBouncy)
			{
				Debug.Log("Player hasn't unlocked Bouncy ammo yet.");
			}
		}

		if (Input.GetKeyDown(KeyCode.K))
		{
			if (PS.playerHasUnlockedSpeedy)
			{
				ammoType = AmmoType.Speedy;
				Debug.Log("Ammo type: " + ammoType);
			}
			else if (!PS.playerHasUnlockedSpeedy)
			{
				Debug.Log("Player hasn't unlocked Speedy ammo yet.");
			}
		}

		if (Input.GetKeyDown(KeyCode.L))
		{
			if (PS.playerHasUnlockedSticky)
			{
				ammoType = AmmoType.Sticky;
				Debug.Log("Ammo type: " + ammoType);	
			}
			else if (!PS.playerHasUnlockedSticky)
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
				ColourIndicator.GetComponent<SpriteRenderer>().material.color = FB.BouncyColour;
				AmmoNameText.color = FB.BouncyColour;
				break;
			case AmmoType.Speedy:
				//ColourIndicator.GetComponent<TilemapRenderer>().color = new Color(255, 153, 0);
				ColourIndicator.GetComponent<SpriteRenderer>().material.color = FB.SpeedyColour;
				AmmoNameText.color = FB.SpeedyColour;
				//SPC.playerSpeed = 10;
				break;
			case AmmoType.Sticky:
				//ColourIndicator.GetComponent<SpriteRenderer>().color = new Color(125, 38, 205);
				ColourIndicator.GetComponent<SpriteRenderer>().color = FB.StickyColour;
				AmmoNameText.color = FB.StickyColour;
				//ColourIndicator.GetComponent<TilemapRenderer>().color = new Color32(135, 36, 183, 255);
				break;
			case AmmoType.Clear:
				ColourIndicator.GetComponent<SpriteRenderer>().color = FB.ClearColour;
				AmmoNameText.color = FB.ClearColour;
				break;
		}
	}

	/*private void OnCollisionEnter2D(Collision2D other)
	{
		switch (ammoType)
		{
			case AmmoType.Bouncy:
			{
				//ColourIndicator.GetComponent<TilemapRenderer>().color = Color.cyan;
				//Debug.Log("Current Ammo: Bouncy");
				if (other.gameObject.tag == "Ground" || other.gameObject.layer.ToString() == "Ground")
				{
					other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = BouncyMaterial;
					other.gameObject.GetComponent<TilemapRenderer>().color = Color.cyan;
				}

				break;
			}
			case AmmoType.Speedy:
			{
				//ColourIndicator.GetComponent<TilemapRenderer>().color = new Color(255, 153, 0);
				//Debug.Log("Current Ammo: Speedy");
				if (other.gameObject.tag == "Ground" || other.gameObject.layer.ToString() == "Ground")
				{
					
					other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = SpeedyMaterial;
					//other.gameObject.GetComponent<TilemapRenderer>().color = new Color(255, 153, 0);
					ColourIndicator.GetComponent<TilemapRenderer>().color = Color.yellow;
				}

				break;
			}
			case AmmoType.Sticky:
			{
				//ColourIndicator.GetComponent<TilemapRenderer>().color = new Color(125, 38, 205);
				//Debug.Log("Current Ammo: Sticky");
				if (other.gameObject.tag == "Ground" || other.gameObject.layer.ToString() == "Ground")
				{
					other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = StickyMaterial;
					//other.gameObject.GetComponent<TilemapRenderer>().color = new Color(125, 38, 205);
					ColourIndicator.GetComponent<TilemapRenderer>().color = Color.magenta;
				}

				break;
			}

			case AmmoType.Clear:
			{
				//ColourIndicator.GetComponent<TilemapRenderer>().color = Color.cyan;
				//Debug.Log("Current Ammo: Bouncy");
				if (other.gameObject.tag == "Ground" || other.gameObject.layer.ToString() == "Ground")
				{
					other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = null;
					other.gameObject.GetComponent<TilemapRenderer>().color = Color.grey;
				}

				break;
			}
		}
	}*/

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Ground" || other.gameObject.layer.ToString() == "Ground")
		{
			switch (ammoType)
			{
				case AmmoType.Bouncy:
					inkPuddlePrefab.GetComponent<SpriteRenderer>().color = FB.BouncyColour;
					foreach (ContactPoint2D ballHit in other.contacts)
					{
						//Vector2 hitTileVector2 = ballHit.point;
						//tileToChange = PlatformsTilemap.GetTile(new Vector3Int());
						Debug.Log("Hit a tile at position:" + ballHit.point);
						Vector2 hitPos = ballHit.point;
						var offsetY = new Vector2(0, 2);
						//Vector2 tileHitPosition = PlatformsGrid.WorldToCell(hitPos); //goddamn vector3int reeeeeeeeeeeeeeeeeee
						GameObject newInkPuddle = Instantiate(inkPuddlePrefab, hitPos + offsetY, Quaternion.identity);
						inkPuddlePrefab.GetComponent<BoxCollider2D>().sharedMaterial = BouncyMaterial;
						inkPuddlePrefab.GetComponent<SpriteRenderer>().color = FB.BouncyColour;
						//TODO: Paint tile that was hit using its position. 
						//HitTile.color = FB.BouncyColour;
					}
					Destroy(gameObject);
					break;
				case AmmoType.Speedy:
					inkPuddlePrefab.GetComponent<TilemapCollider2D>().sharedMaterial = SpeedyMaterial;
					inkPuddlePrefab.GetComponent<TilemapRenderer>().material.color = FB.SpeedyColour;
					//ColourIndicator.GetComponent<TilemapRenderer>().color = Color.yellow;
					ColourIndicator.GetComponent<SpriteRenderer>().color = FB.SpeedyColour;
					Destroy(gameObject);
					break;
				case AmmoType.Sticky:
					other.gameObject.GetComponent<TilemapCollider2D>().sharedMaterial = StickyMaterial;
					other.gameObject.GetComponent<TilemapRenderer>().material.color = FB.StickyColour;
					//ColourIndicator.GetComponent<TilemapRenderer>().color = Color.magenta;
					ColourIndicator.GetComponent<SpriteRenderer>().color = FB.StickyColour;
					Destroy(gameObject);
					break;
				case AmmoType.Clear:
					other.gameObject.GetComponent<TilemapCollider2D>().sharedMaterial = null;
					other.gameObject.GetComponent<TilemapRenderer>().material.color = FB.ClearColour;
					Destroy(gameObject);
					break;
			}
		}

		if (other.gameObject.tag == "ink")
		{
			Destroy(other.gameObject);
			Debug.Log("Can't paint over other ink!");
		}
		
		if (other.gameObject.tag == "Enemy" || other.gameObject.layer.ToString() == "Enemy")
		{
			switch (ammoType)
			{
				case AmmoType.Bouncy:
					//other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = BouncyMaterial;
					other.gameObject.GetComponent<TilemapRenderer>().material.color = FB.BouncyColour;
					if (enemyType == EnemyType.Speedy)
					{
						enemyType = EnemyType.Sticky;
						other.gameObject.GetComponent<TilemapRenderer>().material.color = FB.StickyColour;
					}
					else enemyType = EnemyType.Bouncy;
					Destroy(gameObject);
					break;
				case AmmoType.Speedy:
					//other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = SpeedyMaterial;
					other.gameObject.GetComponent<TilemapRenderer>().material.color = FB.SpeedyColour;

					if (enemyType == EnemyType.Bouncy)
					{
						enemyType = EnemyType.Sticky;
						other.gameObject.GetComponent<TilemapRenderer>().material.color = FB.StickyColour;
					}
					else enemyType = EnemyType.Speedy;
					//ColourIndicator.GetComponent<TilemapRenderer>().color = Color.yellow;
					//ColourIndicator.GetComponent<TilemapRenderer>().color = FB.SpeedyColour;
					Destroy(gameObject);
					break;
				case AmmoType.Sticky:
					//other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = StickyMaterial;
					other.gameObject.GetComponent<TilemapRenderer>().material.color = FB.StickyColour;
					enemyType = EnemyType.Sticky;
					//ColourIndicator.GetComponent<TilemapRenderer>().color = Color.magenta;
					//ColourIndicator.GetComponent<TilemapRenderer>().color = FB.StickyColour;
					Destroy(gameObject);
					break;
				case AmmoType.Clear:
					//other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = null;
					enemyType = EnemyType.Clear;
					other.gameObject.GetComponent<TilemapRenderer>().material.color = FB.ClearColour;
					Destroy(gameObject);
					break;
			}
		}
	}
}