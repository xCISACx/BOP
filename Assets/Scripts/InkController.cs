using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkController : MonoBehaviour
{

	public float speed;
	public PlayerControllerArrows player;

	// Use this for initialization
	void Start ()
	{

		player = FindObjectOfType<PlayerControllerArrows>();

		if (player.transform.localScale.x < 0)
		{
			speed = -speed;
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		
		//GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
		
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		Destroy(gameObject);
	}
}
