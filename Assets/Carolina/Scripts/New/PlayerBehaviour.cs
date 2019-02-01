using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour {

    [SerializeField] GameObject Player;

    [Header("Movement")]
    [SerializeField] float moveSpeed = .1f;
    public float horAxis;
    public float verAxis;
    public float defaultPlayerSpeed = .1f;
    public float VelocityIncrease = 1.55f;
    public float SpeedLimit;
    
    [SerializeField] SpriteRenderer spritePlayer;

    [Header("Aiming")] 
    [SerializeField] Transform AimRotationalPivot;
    [SerializeField] GameObject WeaponArm;
    [SerializeField] GameObject aimTarget;
    [SerializeField] float aimSpeed = 1;
    public GameObject Weapon;
    [SerializeField] private Image _activeCursor;
    [SerializeField] private Transform _weaponArm;
    [SerializeField] private Vector2 _weaponOffset = new Vector2(3f, 0f); 
    [SerializeField] private Sprite _defaultCursor;
    [SerializeField] private Sprite _forbiddenCursor;
    
    public ParticleSystem Nozzle;
    Animator anim;
    Rigidbody2D rb;
    public Vector2 playerJumpPower;
    public bool isGrounded = true;
    public int jumpingAnimationCounter;
    Vector3 playerScale;
    public SpawnInk spawnInk;
    private bool _jumped;
    Vector3 mousePos;
    Ray ray;    
    RaycastHit2D hit;
    public GameManager gameManager;
    public AmmoBehaviour ammoBehaviour;
    
    public bool touchingWall;
    public bool facingRight;
    [SerializeField] private Transform wallCheck;
    //public Transform stickyCheck;
    public CircleCollider2D wallCheckCollider;
    //public Collider2D stickyCheckCollider;
    public TilemapCollider2D wallsCollider;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private LayerMask layerWall;
    //[SerializeField] private LayerMask stickyPuddle;
    [SerializeField] private Vector2 playerWallJumpPower;
    public bool touchingSticky;
    
    [Header("UI")]
    public GameObject mainUI;
    public bool uiActive;
    
    void Start ()
    {
        Time.timeScale = 1;
        if (mainUI == null)
            mainUI = GameObject.FindGameObjectWithTag("UI");
        UICheck();
        playerScale = Player.transform.lossyScale;
        anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        groundCheck = GameObject.Find("Ground Check").transform;
        ammoBehaviour = FindObjectOfType<AmmoBehaviour>();
    }
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
	
	void Update () 
	{
	    UICheck();
	    if (uiActive)
	        spawnInk = FindObjectOfType<SpawnInk>();
	    
	    Player = GameObject.FindGameObjectWithTag("Player");
	    groundCheck = GameObject.Find("Ground Check").transform;
	    if (groundCheck == null)
	    {
	        Debug.Log("Ground Check not found");
	    }
	    isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, layerGround);
        Aiming();
	    ammoBehaviour.AmmoLimiter();
	    if (Input.GetKeyDown(KeyCode.Space))
	        _jumped = true;
	    WallJumping();
	    if (touchingSticky)
	    {
	        PlayerMovement(false);
	    }
	}

    void FixedUpdate()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        groundCheck = GameObject.Find("Ground Check").transform;
        if (groundCheck == null)
        {
            Debug.Log("Ground Check not found");
        }
        //touchingWall = Physics2D.Linecast(transform.position, wallCheck.position, layerWall); //TODO: Fix the wall jumping mechanic.
        touchingWall = wallCheckCollider.IsTouchingLayers(layerWall);
        //touchingSticky = Physics2D.Linecast(transform.position, stickyCheck.position, stickyPuddle);
        //touchingSticky = stickyCheckCollider.IsTouchingLayers(stickyPuddle);
        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, layerGround);
        anim.SetBool("can run", true);
        PlayerMovement(true);
    }
    
    void PlayerMovement(bool isEnabled)
    {
        if (isEnabled)
        {
            horAxis = Input.GetAxis("Horizontal");
            verAxis = Input.GetAxis("Vertical");
            
            Player.transform.position = new Vector2(Player.transform.position.x + horAxis * moveSpeed, Player.transform.position.y);

            if (horAxis > 0)
            {
                Player.transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z); //if the player is facing right, the x scale is positive
                facingRight = true;
            }
            else if (horAxis < 0)
            {
                Player.transform.localScale = new Vector3(-playerScale.x, playerScale.y, playerScale.z); //if the player is NOT facing right, the x scale is negative
                facingRight = false;
            }
            
            
            if (horAxis < 0.05 || horAxis > -0.05)
            {
                //anim.SetFloat("speed", 0);
                anim.SetBool("running", false);
            }

            if (horAxis > 0.05 || horAxis < -0.05)
            {
                //anim.SetFloat("speed", 1);
                anim.SetBool("running", true);
            }
            
            if (_jumped)
            {
                Jump();
                _jumped = false;
            }
        }
    }

    void Aiming ()
    {
        if (Input.GetMouseButton(0))
        {
            if (!gameManager.playerHasGun) return;
            
            ammoBehaviour.NoAmmo();
            ammoBehaviour.SpendAmmo(1);
            //Debug.Log("Spending ammo...");
            var em = Nozzle.emission;
            em.enabled = true;
            
            Vector2 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector3 axis = new Vector3(0, 0, 1);
        }
        else
        {
            var em = Nozzle.emission;
            em.enabled = false;
        }
    }
    
    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(playerJumpPower, ForceMode2D.Impulse);
            anim.SetBool("jumping", true);
            Debug.Log("Playing jumping animation.");
            isGrounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Puddle") || other.gameObject.layer.ToString() == "Puddle")
        {
            if (other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial == spawnInk.SpeedyMaterial)
            {
                anim.SetBool("sliding", true);
                rb.drag = 0;
                moveSpeed *= VelocityIncrease;
                if (moveSpeed > SpeedLimit)
                {
                    moveSpeed = SpeedLimit;
                }
            }
        }

        if (other.gameObject.CompareTag("Puddle") || other.gameObject.layer.ToString() == "Puddle")
        {
            if (other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial == spawnInk.StickyMaterial)
            {
                touchingSticky = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    { 
        if (other.gameObject.CompareTag("Ground") || other.gameObject.layer.ToString() == "Ground")
        {
            isGrounded = true;
            anim.SetBool("sliding", false);
            anim.SetBool("jumping", false);
            rb.drag = 0.0f;
            moveSpeed = defaultPlayerSpeed;

            if (other.gameObject.GetComponent<TilemapCollider2D>().sharedMaterial != spawnInk.BouncyMaterial)
            {
                rb.drag = 0.5f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Puddle") || other.gameObject.layer.ToString() == "Puddle")
        {
            if (other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial == spawnInk.StickyMaterial)
            {
                //anim.SetBool("grabbing wall", false);
                touchingSticky = false;
                //rb.gravityScale = 9.8f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Acid"))
        {
            isGrounded = false;
            gameManager.KillPlayer();
        }
    }

    public void onHover(Ray ray, RaycastHit2D hit)
    {
        if(hit.collider == null)
        {
            Debug.Log("nothing hit");
        }      
        
        if (Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity))
        {
            print(hit.collider.name);
        }
    }

    void WallJumping()
    {
        if (touchingWall && touchingSticky) //TODO: The animation won't work correctly because we can't detect if the player is touching the sticky ink correctly as the puddles don't spawn on the edge of the wall.
        {
            
            //anim.SetBool("can run", false);
            //anim.SetBool("grabbing wall", true);
            //anim.SetBool("jumping", false);
            //anim.SetBool("running", false);
            //float slidingVelocity = -1f;
            //rb.gravityScale = 0;
            //Debug.Log("changed sliding velocity");
            //rb.velocity = new Vector2(rb.velocity.x, slidingVelocity);
            //Debug.Log(rb.velocity);
            //rb.gravityScale = -1f; //new mechanic?

            //Debug.Log("Stage1: touching sticky wall, sliding down");
            if ((horAxis > 0.05f && facingRight && !isGrounded) || (horAxis < -0.05f && !facingRight && isGrounded))
            {
                
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (facingRight)
                    {
                        //Debug.Log("Wall jumping to the left");
                        rb.AddForce(playerWallJumpPower, ForceMode2D.Force);
                        //Player.transform.localScale = new Vector3(-playerScale.x, playerScale.y, playerScale.z); //TODO: flip player automatically after they wall jump.
                    }
                    else
                    {
                        //Debug.Log("Wall jumping to the right");
                        rb.AddForce( new Vector2(-playerWallJumpPower.x,playerWallJumpPower.y), ForceMode2D.Force);
                        //Player.transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z);
                    }
                }
            }
        }
    }
    
    void UICheck()
    {
        if (uiActive)
        {
            mainUI.SetActive(true);
        }
        
        else
        {
            mainUI.SetActive(false);
        }
    }
}


