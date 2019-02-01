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
    public bool playerHasGun;
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
    public SpawnInk SI;
    private bool _jumped;
    Vector3 mousePos;
    Ray ray;    
    RaycastHit2D hit;
    public GameManager gameManager;
    
    public bool touchingWall;
    public bool facingRight;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private LayerMask layerWall;
    [SerializeField] private Vector2 playerWallJumpPower;
    public bool touchingSticky;

    public bool canReduceAmmo = true;
    public float ammoSpendingCooldownTime;
    
    [Header("Ammo")]
    public int bouncyAmmoPercent = 1;
    public int speedyAmmoPercent = 1;
    public int stickyAmmoPercent = 1;
    public int clearAmmoPercent = 1;
	
    public int currentBouncyAmmo = 20;
    public int currentSpeedyAmmo = 20;
    public int currentStickyAmmo = 20;
    public int currentClearAmmo = 20;
	
    public int initialBouncyAmmo = 5;
    public int initialSpeedyAmmo = 5;
    public int initialStickyAmmo = 5;
    public int initialClearAmmo = 5;
	
    public int maxBouncyAmmo = 20;
    public int maxSpeedyAmmo = 20;
    public int maxStickyAmmo = 20;
    public int maxClearAmmo = 20;
    
    [Header("UI")]
    public GameObject mainUI;
    public bool uiActive;
    
    void Start ()
    {
        if (mainUI == null)
            mainUI = GameObject.FindGameObjectWithTag("UI");
        UICheck();
        canReduceAmmo = true;
        playerScale = Player.transform.lossyScale;
        anim = GetComponent<Animator>();
        if (uiActive)
            SI = GameObject.Find("InkSpray").GetComponent<SpawnInk>();
        Player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        groundCheck = GameObject.Find("Ground Check").transform;
    }
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
	
	void Update () 
	{
	    Player = GameObject.FindGameObjectWithTag("Player");
	    groundCheck = GameObject.Find("Ground Check").transform;
	    if (groundCheck == null)
	    {
	        Debug.Log("Ground Check not found");
	    }
	    isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, layerGround);
	    if (uiActive)
	    {
	        
	    }
        Aiming();
	    AmmoLimiter();
	    if (Input.GetKeyDown(KeyCode.Space))
	        _jumped = true;
	    WallJumping();
	}

    void FixedUpdate()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        groundCheck = GameObject.Find("Ground Check").transform;
        if (groundCheck == null)
        {
            Debug.Log("Ground Check not found");
        }
        touchingWall = Physics2D.Linecast(transform.position, wallCheck.position, layerWall);
        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, layerGround);
        PlayerMovement(true);
    }
    
    void PlayerMovement(bool isEnabled)
    {

        // spritePlayer.flipX = true;

        if (isEnabled)
        {
            horAxis = Input.GetAxis("Horizontal");
            verAxis = Input.GetAxis("Vertical");
            
            Player.transform.position = new Vector2(Player.transform.position.x + horAxis * moveSpeed, Player.transform.position.y);

            if (horAxis > 0)
            {
                Player.transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z); //spritePlayer.flipX = true;
                facingRight = true;
            }
            else if (horAxis < 0)
            {
                Player.transform.localScale = new Vector3(-playerScale.x, playerScale.y, playerScale.z); // spritePlayer.flipX = false;
                facingRight = false;
            }
            
            
            if (horAxis < 0.05 || horAxis > -0.05)
            {
                anim.SetFloat("speed", 0);
            }

            if (horAxis > 0.05 || horAxis < -0.05)
            {
                anim.SetFloat("speed", 1);
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
            NoAmmo();
            SpendAmmo(1);
            Debug.Log("Spending ammo...");
            var em = Nozzle.emission;
            em.enabled = true;
            
            Vector2 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            //Debug.Log("Angle: " + angle);

            Vector3 axis = new Vector3(0, 0, 1);

            //Vector3 lookAtPos = Vector3.zero;
            //lookAtPos.z = aimTarget.transform.position.y;

           // WeaponArm.transform.RotateAround(AimRotationalPivot.position, axis, angle);
            //WeaponArm.transform.LookAt(lookAtPos);
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
            anim.SetInteger("State", 5);
            Debug.Log("Playing jumping animation.");
            isGrounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Puddle") || other.gameObject.layer.ToString() == "Puddle")
        {
            if (other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial == SI.SpeedyMaterial)
            {
                anim.SetInteger("State", 6);
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
            if (other.gameObject.GetComponent<BoxCollider2D>().sharedMaterial == SI.StickyMaterial)
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
            
            anim.SetInteger("State", 0);
            rb.drag = 0.0f;
            Debug.Log("Landed on the ground.");
            Debug.Log("Not touching the speedy ink");
            moveSpeed = defaultPlayerSpeed;

            if (other.gameObject.GetComponent<TilemapCollider2D>().sharedMaterial != SI.BouncyMaterial)
            {
                rb.drag = 0.5f;
            }
        }
        
        /*if (other.gameObject.CompareTag("Ground") || other.gameObject.layer.ToString() == "Ground" || other.gameObject.CompareTag("Puddle") || other.gameObject.layer.ToString() == "Puddle")
        {
              
            isGrounded = true;
            //Debug.Log("Player is grounded.");
            //anim.SetInteger("State", 0);
            //Debug.Log("Stopped jumping.");
            
        }
        //else
        {
            isGrounded = false;
            Debug.Log("Player is NOT grounded.");
        }*/
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        touchingSticky = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Acid"))
        {
            isGrounded = false;
            gameManager.KillPlayer();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
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
        if (touchingWall && touchingSticky)
        {
            float slidingVelocity = -1f;
            //rb.velocity = (new Vector2(slidingVelocity, 0f));

            Debug.Log("Stage1: touching sticky wall, sliding down");
            if ((horAxis > 0.0f && facingRight && !isGrounded) || (horAxis < 0.0f && !facingRight && !isGrounded))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (facingRight)
                    {
                        Debug.Log("Wall jumping to the left");
                        rb.AddForce(playerWallJumpPower, ForceMode2D.Force);
                        //Player.transform.localScale = new Vector3(-playerScale.x, playerScale.y, playerScale.z);
                    }
                    else
                    {
                        Debug.Log("Wall jumping to the right");
                        //playerWallJumpPower.x = -playerWallJumpPower.x;
                        rb.AddForce( new Vector2(-playerWallJumpPower.x,playerWallJumpPower.y), ForceMode2D.Force);
                        //Player.transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z);
                    }
                }
            }
        }
    }
    
    public void SpendAmmo(int ammo)
    {
        switch (SI.ammoType)
        {
            case SpawnInk.AmmoType.Bouncy:
                if (canReduceAmmo)
                {
                    currentBouncyAmmo -= ammo;
                    if (currentBouncyAmmo != 0)
                    {
                        gameManager.ReduceAmmoBar();   
                    }
                    StartCoroutine(JustSpentAmmo());
                }
                break;
            case SpawnInk.AmmoType.Speedy:
                if (canReduceAmmo)
                {
                    currentSpeedyAmmo -= ammo;
                    if (currentSpeedyAmmo != 0)
                    {
                        gameManager.ReduceAmmoBar();   
                    }
                    StartCoroutine(JustSpentAmmo());
                }
                break;
            case SpawnInk.AmmoType.Sticky:
                if (canReduceAmmo)
                {
                    currentStickyAmmo -= ammo;
                    if (currentStickyAmmo != 0)
                    {
                        gameManager.ReduceAmmoBar();   
                    }
                    StartCoroutine(JustSpentAmmo());
                }
                break;
            case SpawnInk.AmmoType.Clear:
                if (canReduceAmmo)
                {
                    currentClearAmmo -= ammo;
                    if (currentClearAmmo != 0)
                    {
                        gameManager.ReduceAmmoBar();   
                    }
                    StartCoroutine(JustSpentAmmo());
                }
                break;
        }
    }
						
    IEnumerator JustSpentAmmo()
    {
        canReduceAmmo = false;
        yield return new WaitForSeconds(ammoSpendingCooldownTime);
        canReduceAmmo = true;
    }

    public void AmmoLimiter()
    {
        if (currentBouncyAmmo < 0)
            currentBouncyAmmo = 0;
        if (currentSpeedyAmmo < 0)
            currentSpeedyAmmo = 0;
        if (currentStickyAmmo < 0)
            currentStickyAmmo = 0;
        if (currentClearAmmo < 0)
            currentClearAmmo = 0;
        if (currentBouncyAmmo > maxBouncyAmmo)
            currentBouncyAmmo = maxBouncyAmmo;
        if (currentSpeedyAmmo > maxBouncyAmmo)
            currentSpeedyAmmo = maxBouncyAmmo;
        if (currentStickyAmmo > maxBouncyAmmo)
            currentStickyAmmo = maxBouncyAmmo;
        if (currentClearAmmo > maxBouncyAmmo)
            currentClearAmmo = maxBouncyAmmo;
    }

    public void NoAmmo()
    {
        if (currentBouncyAmmo == 0)
        {
            Debug.Log("No bouncy ammo left");
            return;
        }
        if (currentSpeedyAmmo == 0)
        {
            Debug.Log("No speedy ammo left");
            return;
        }
        if (currentStickyAmmo == 0)
        {
            Debug.Log("No sticky ammo left");
            return;
        }
        if (currentClearAmmo == 0)
        {
            Debug.Log("No clear ammo left");
            return;
            
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


