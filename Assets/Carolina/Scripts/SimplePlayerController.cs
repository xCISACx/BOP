using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{	

	public float playerSpeed;
	public float defaultPlayerSpeed = 5;
	public bool facingRight = true;
	public int playerJumpPower = 1250;
	private float moveX;
	public bool SpeedLocked;
	Rigidbody2D rb;
	public Transform firePoint;
	public LayerMask notToHit;
	public GameObject ball;
	public float runningSpeed;
	public InkBehaviour IB;
	
	// Use this for initialization
	void Start () {
		
	}

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{

		PlayerMove();
	}

	private void OnCollisionStay2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Ground") || other.gameObject.layer.ToString() == "Ground")
		{
			if (other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial == IB.SpeedyMaterial && !SpeedLocked)
			{
				playerSpeed *= 2;
				SpeedLocked = true;
			}
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Ground") || other.gameObject.layer.ToString() == "Ground")
		{
			Debug.Log("Left the ground.");
			if (other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial == IB.SpeedyMaterial)
			{
				Debug.Log("Not touching the speedy ink");
				playerSpeed = defaultPlayerSpeed;
				SpeedLocked = false;
			}
		}
	}

	void PlayerMove()
	{
		moveX = Input.GetAxis("Horizontal");
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			playerSpeed *= runningSpeed;
		}
		
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			playerSpeed /= runningSpeed;
		}

		if (moveX < 0 && facingRight)
		{
			FlipPlayer();
		}
		else if (moveX > 0 && !facingRight)
		{
			FlipPlayer();
		}
		
		rb.velocity = new Vector2(moveX * playerSpeed, rb.velocity.y);
	}

	void Jump()
	{
		rb.AddForce(Vector2.up * playerJumpPower);
	}

	void FlipPlayer()
	{
		facingRight = !facingRight;
		Vector2 localScale = gameObject.transform.localScale;
		localScale.x *= -1;
		transform.localScale = localScale;
	}
}
