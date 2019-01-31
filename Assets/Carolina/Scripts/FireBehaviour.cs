using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class FireBehaviour : MonoBehaviour
{

	public GameObject Ball;

	public float FiringForce;

	//public float firePointOffset = 0.1f;
	public GameObject FirePoint;
	public Transform firePoint;
	public PlayerControllerArrows player;
	public InkBehaviour inkBehaviour;
	public Color32 BouncyColour;
	public Color32 SpeedyColour;
	public Color32 StickyColour;
	public Color32 ClearColour;
	public Animator Anim;
	public Material test;
	public bool playerHasGun = false;
	public PlayerStats PS;
	public LayerMask notToHit;
	public InkBehaviour.AmmoType ammoType;
	private Vector2 mousePosition;
	private Vector2 firePointPosition;
	public float shootingForce;
	//public GameObject MCArm;

	// Use this for initialization
	void Start()
	{
		//DisableGunRendering();
		Anim = player.GetComponent<Animator>();

		if (firePoint != null)
		{
			Debug.Log("firePoint set!");
		}

		float firePointLocalScaleX = firePoint.transform.localScale.x;

		if (player.transform.localScale.x < 0)
		{
			firePointLocalScaleX = -firePointLocalScaleX;
		}

	}

	// Update is called once per frame
	void Update()
	{
		if (playerHasGun)
		{
			if (Input.GetButtonDown("Fire1"))
			{
				StartShootingWithTheMouse();
				//Anim.SetInteger("State", 5);
			}
			else if (Input.GetButtonUp("Fire1"))
			{
				Anim.SetInteger("State", 0);
			}
		}
		Debug.DrawLine(firePointPosition, mousePosition);
	}

	void StartShooting()
	{
		firePoint = transform.Find("PlayerFirePoint");
		//Anim.SetInteger("State", 5);
		GameObject newBall = Instantiate(Ball, firePoint.position, firePoint.rotation);
		switch (inkBehaviour.ammoType)
		{
			case InkBehaviour.AmmoType.Bouncy:
				if (PS.CurrentBouncyAmmo != 0)
				{
					newBall.GetComponent<SpriteRenderer>().color = BouncyColour;
					PS.CurrentBouncyAmmo--;	
				}
				else if (PS.CurrentBouncyAmmo == 0)
				{
					ammoType = InkBehaviour.AmmoType.Clear;
					Debug.Log("No Bouncy ammo!");
				}
				break;
			case InkBehaviour.AmmoType.Speedy:
				newBall.GetComponent<SpriteRenderer>().color = SpeedyColour;
				break;
			case InkBehaviour.AmmoType.Sticky:
				newBall.GetComponent<SpriteRenderer>().color = StickyColour;
				break;
			case InkBehaviour.AmmoType.Clear:
				newBall.GetComponent<SpriteRenderer>().color = ClearColour;
				break;
		}

		newBall.GetComponent<Rigidbody2D>().AddForce(transform.right * FiringForce);
	}

	void StartShootingWithTheMouse()
	{
		if (firePoint == null)
		{
			Debug.Log("FirePoint not found, why?!?");
		}
		
		mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition-firePointPosition, 100, notToHit);

		GameObject newBall = Instantiate(Ball, firePoint.position, firePoint.rotation);
		var newBallRigidbody = newBall.GetComponent<Rigidbody2D>();
		newBallRigidbody.angularVelocity *= shootingForce;
		switch (inkBehaviour.ammoType)
		{
			case InkBehaviour.AmmoType.Bouncy:
				if (PS.CurrentBouncyAmmo != 0)
				{
					newBall.GetComponent<SpriteRenderer>().color = BouncyColour;
					PS.CurrentBouncyAmmo--;	
				}
				else if (PS.CurrentBouncyAmmo == 0)
				{
					ammoType = InkBehaviour.AmmoType.Clear;
					Debug.Log("No Bouncy ammo!");
				}
				break;
			case InkBehaviour.AmmoType.Speedy:
				newBall.GetComponent<SpriteRenderer>().color = SpeedyColour;
				break;
			case InkBehaviour.AmmoType.Sticky:
				newBall.GetComponent<SpriteRenderer>().color = StickyColour;
				break;
			case InkBehaviour.AmmoType.Clear:
				newBall.GetComponent<SpriteRenderer>().color = ClearColour;
				break;
		}

		newBall.GetComponent<Rigidbody2D>().AddForce(transform.right * FiringForce);
	}

	void DisableGunRendering()
	{
		FirePoint.GetComponent<SpriteRenderer>().enabled = false;
		inkBehaviour.ColourIndicator.GetComponent<SpriteRenderer>().enabled = false;
	}

	void EnableGunRendering()
	{
		FirePoint.GetComponent<SpriteRenderer>().enabled = true;
		inkBehaviour.ColourIndicator.GetComponent<SpriteRenderer>().enabled = true;
	}
}
