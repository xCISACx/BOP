using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class FireBehaviour : MonoBehaviour
{

	public GameObject Ball;
	public float FiringForce;
	//public float firePointOffset = 0.1f;
	public Transform firePoint;
	public PlayerControllerArrows player;
	public InkBehaviour inkBehaviour;
	public Color32 BouncyColour;
	public Color32 SpeedyColour;
	public Color32 StickyColour;
	public Color32 ClearColour;
	

	// Use this for initialization
	void Start () {
		
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
	void Update ()
	{

		StartShooting();

	}

	void StartShooting()
	{
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			firePoint = transform.Find("PlayerFirePoint");
			GameObject newBall = Instantiate(Ball, firePoint.position, firePoint.rotation);
			switch (inkBehaviour.ammoType)
			{
				case InkBehaviour.AmmoType.Bouncy:
					newBall.GetComponent<SpriteRenderer>().color = BouncyColour;
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
	}
}
