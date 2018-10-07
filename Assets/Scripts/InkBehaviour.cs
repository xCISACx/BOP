using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InkBehaviour : MonoBehaviour
{
	public enum AmmoType
	{
		Bouncy,
		Speedy,
		Sticky,
		Clear
	}

	public PhysicsMaterial2D BouncyMaterial;
	public PhysicsMaterial2D SpeedyMaterial;
	public PhysicsMaterial2D StickyMaterial;
	public GameObject ColourIndicator;
	public AmmoType ammoType;
	public SimplePlayerController SPC;
	public FireBehaviour FB;
	public TextMeshProUGUI AmmoNameText;
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
			ammoType = AmmoType.Bouncy;
			Debug.Log("Ammo type: " + ammoType);
		}
		
		if (Input.GetKeyDown(KeyCode.K))
		{
			ammoType = AmmoType.Speedy;
			Debug.Log("Ammo type: " + ammoType);
		}
		
		if (Input.GetKeyDown(KeyCode.L))
		{
			ammoType = AmmoType.Sticky;
			Debug.Log("Ammo type: " + ammoType);
		}
		
		if (Input.GetKeyDown(KeyCode.P))
		{
			ammoType = AmmoType.Clear;
			Debug.Log("Ammo type: " + ammoType);
		}
		
		switch (ammoType)
		{
			case AmmoType.Bouncy:
				ColourIndicator.GetComponent<SpriteRenderer>().color = FB.BouncyColour;
				AmmoNameText.color = FB.BouncyColour;
				break;
			case AmmoType.Speedy:
				//ColourIndicator.GetComponent<SpriteRenderer>().color = new Color(255, 153, 0);
				ColourIndicator.GetComponent<SpriteRenderer>().color = FB.SpeedyColour;
				AmmoNameText.color = FB.SpeedyColour;
				//SPC.playerSpeed = 10;
				break;
			case AmmoType.Sticky:
				//ColourIndicator.GetComponent<SpriteRenderer>().color = new Color(125, 38, 205);
				ColourIndicator.GetComponent<SpriteRenderer>().color = FB.StickyColour;
				AmmoNameText.color = FB.StickyColour;
				//ColourIndicator.GetComponent<SpriteRenderer>().color = new Color32(135, 36, 183, 255);
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
				//ColourIndicator.GetComponent<SpriteRenderer>().color = Color.cyan;
				//Debug.Log("Current Ammo: Bouncy");
				if (other.gameObject.tag == "Ground" || other.gameObject.layer.ToString() == "Ground")
				{
					other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = BouncyMaterial;
					other.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
				}

				break;
			}
			case AmmoType.Speedy:
			{
				//ColourIndicator.GetComponent<SpriteRenderer>().color = new Color(255, 153, 0);
				//Debug.Log("Current Ammo: Speedy");
				if (other.gameObject.tag == "Ground" || other.gameObject.layer.ToString() == "Ground")
				{
					
					other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = SpeedyMaterial;
					//other.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 153, 0);
					ColourIndicator.GetComponent<SpriteRenderer>().color = Color.yellow;
				}

				break;
			}
			case AmmoType.Sticky:
			{
				//ColourIndicator.GetComponent<SpriteRenderer>().color = new Color(125, 38, 205);
				//Debug.Log("Current Ammo: Sticky");
				if (other.gameObject.tag == "Ground" || other.gameObject.layer.ToString() == "Ground")
				{
					other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = StickyMaterial;
					//other.gameObject.GetComponent<SpriteRenderer>().color = new Color(125, 38, 205);
					ColourIndicator.GetComponent<SpriteRenderer>().color = Color.magenta;
				}

				break;
			}

			case AmmoType.Clear:
			{
				//ColourIndicator.GetComponent<SpriteRenderer>().color = Color.cyan;
				//Debug.Log("Current Ammo: Bouncy");
				if (other.gameObject.tag == "Ground" || other.gameObject.layer.ToString() == "Ground")
				{
					other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = null;
					other.gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
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
					other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = BouncyMaterial;
					other.gameObject.GetComponent<SpriteRenderer>().color = FB.BouncyColour;
					Destroy(gameObject);
					break;
				case AmmoType.Speedy:
					other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = SpeedyMaterial;
					//other.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 153, 0);
					//ColourIndicator.GetComponent<SpriteRenderer>().color = Color.yellow;
					ColourIndicator.GetComponent<SpriteRenderer>().color = FB.SpeedyColour;
					Destroy(gameObject);
					break;
				case AmmoType.Sticky:
					other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = StickyMaterial;
					//other.gameObject.GetComponent<SpriteRenderer>().color = new Color(125, 38, 205);
					//ColourIndicator.GetComponent<SpriteRenderer>().color = Color.magenta;
					ColourIndicator.GetComponent<SpriteRenderer>().color = FB.StickyColour;
					Destroy(gameObject);
					break;
				case AmmoType.Clear:
					other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = null;
					other.gameObject.GetComponent<SpriteRenderer>().color = FB.ClearColour;
					Destroy(gameObject);
					break;
			}
		}
	}	
}