using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Tilemaps;

public class SimplePlayerController : MonoBehaviour
{	

	public float playerSpeed;
	public float defaultPlayerSpeed = 15;
	public float VelocityIncrease = 1.55f;
	public bool facingRight = true;
	public int playerJumpPower = 1250;
	private float moveX;
	//public bool SpeedLocked;
	private float SpeedLimit; 
	Rigidbody2D rb;
	public Transform firePoint;
	public LayerMask notToHit;
	public GameObject ball;
	public float runningSpeed;
	public InkBehaviour IB;
	public FireBehaviour FB;
	public PlayerStats PS;
	
	[SerializeField]
	private TilemapCollider2D[] colliders;
	private int currentColliderIndex = 0;
	
	// Use this for initialization
	void Start ()
	{
		FB.playerHasGun = false;
		PS.playerHasUnlockedBouncy = false;
		PS.playerHasUnlockedSpeedy = false;
		PS.playerHasUnlockedSticky = false;
		SpeedLimit = defaultPlayerSpeed * VelocityIncrease;

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
			if (other.gameObject.GetComponent<TilemapCollider2D>().sharedMaterial == IB.SpeedyMaterial)
			{
				rb.drag = 0;
				playerSpeed *= VelocityIncrease;
				if (playerSpeed > SpeedLimit)
				{
					playerSpeed = SpeedLimit;
				}
			}
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		rb.drag = 0.0f;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Ground") || other.gameObject.layer.ToString() == "Ground")
		{
			rb.drag = 0.0f;
			Debug.Log("Landed on the ground.");
			if (other.gameObject.GetComponent<TilemapCollider2D>().sharedMaterial != IB.SpeedyMaterial)
			{
				Debug.Log("Not touching the speedy ink");
				playerSpeed = defaultPlayerSpeed;
			}

			if (other.gameObject.GetComponent<TilemapCollider2D>().sharedMaterial != IB.BouncyMaterial)
			{
				rb.drag = 0.5f;
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
		
		if( (moveX > 0.05f) || (moveX < -0.05f))
		rb.velocity = new Vector2(moveX * playerSpeed, rb.velocity.y);
	}

	void Jump()
	{
		rb.AddForce(Vector2.up * playerJumpPower);
	}

	void FlipPlayer()
	{
		facingRight = !facingRight;
		transform.Rotate(0f, 180, 0f);
	}
	
	public void SetColliderForSprite( int spriteNum )
	{
		//Debug.Log("switching from " + currentColliderIndex + " to " + spriteNum);
		colliders[currentColliderIndex].enabled = false;
		currentColliderIndex = spriteNum;
		colliders[currentColliderIndex].enabled = true;
	}
}
