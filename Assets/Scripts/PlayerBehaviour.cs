using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerBehaviour : MonoBehaviour {

    [SerializeField] GameObject thePlayer;

    [Header("Movement")]
    [SerializeField] float moveSpeed = .1f;
    [SerializeField] SpriteRenderer spritePlayer;
    [Header("Aiming")]
    [SerializeField] float aimSpeed = 1;
    [SerializeField] Transform AimRotationalPivot;
    [SerializeField] GameObject WeaponArm;
    [SerializeField] GameObject aimTarget;
    public GameObject Weapon;
    public ParticleSystem Nozzle;
    Animator anim;
    public float horAxis;
    public float verAxis;
    Rigidbody2D rb;
    public Vector2 playerJumpPower;
    public bool isGrounded;
    public int jumpingAnimationCounter;
    Vector3 playerScale;
    public Animation Jumping;
    public float defaultPlayerSpeed = .1f;
    public float VelocityIncrease = 1.55f;
    public float SpeedLimit;
    public SpawnInk SI;
    private bool _jumped;
    public GameObject AimSightPrefab;
    public GameObject ForbiddenAimSightPrefab;
    public GameObject defaulttarget;
    public GameObject target;
    public GameObject forbiddentarget;
    Vector3 mousePos;
    Ray ray;    
    RaycastHit2D hit;
    
    void Start () 
    {
        playerScale = thePlayer.transform.lossyScale;
        anim = GetComponent<Animator>();
        target = (GameObject)Instantiate(AimSightPrefab);
        forbiddentarget = (GameObject) Instantiate(ForbiddenAimSightPrefab);
    }
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
	
	void Update () 
	{
        Aiming();
	    Cursor.visible = false;
	    if (Input.GetKeyDown(KeyCode.Space))
	        _jumped = true;
	    mousePos = Input.mousePosition;
	    mousePos.z = 10f;
 
	    target.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
	    
	    Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
	    Vector2 WeaponArmPosition = new Vector2(WeaponArm.transform.position.x + 1, WeaponArm.transform.position.y);
	    RaycastHit2D hit = Physics2D.Raycast(WeaponArmPosition, mousePosition - WeaponArmPosition, 5);
	    Debug.DrawLine (WeaponArmPosition, mousePosition, Color.red);
	    if (hit.transform != null)
	    {
	        Debug.Log("Collider hit:" + hit.transform.name);
	    }
	    else return;

	    if (hit.collider.gameObject.CompareTag("Acid") || hit.transform.gameObject.layer == LayerMask.NameToLayer("Acid"))
	    {
	        Debug.Log("Aiming at Acid");
	        target = forbiddentarget;
	    }
	    else if (!hit.collider.gameObject.CompareTag("Acid") || hit.transform.gameObject.layer != LayerMask.NameToLayer("Acid") || hit.collider.gameObject == null)
	    {
	        Debug.Log("Not hitting any forbidden surfaces, changing sight");
	        target = defaulttarget;
	        target.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
	    }
	}

    void FixedUpdate()
    {
        PlayerMovement(true);
    }




    void PlayerMovement(bool isEnabled)
    {

        // spritePlayer.flipX = true;

        if (isEnabled)
        {
            horAxis = Input.GetAxis("Horizontal");
            verAxis = Input.GetAxis("Vertical");
            
            thePlayer.transform.position = new Vector2(thePlayer.transform.position.x + horAxis * moveSpeed, thePlayer.transform.position.y);

            if (horAxis > 0)
            {
                thePlayer.transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z); //spritePlayer.flipX = true;
            }
            else if (horAxis < 0)
            {
                thePlayer.transform.localScale = new Vector3(-playerScale.x, playerScale.y, playerScale.z); // spritePlayer.flipX = false;
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
        if (Input.GetMouseButton(0)) // Bool : Has weapon?
        {
            var em = Nozzle.emission;
            em.enabled = true;
            
            Vector2 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            mousePos.x = mousePos.x - 0.5f;
            mousePos.y = mousePos.y - 0.5f;

            mousePos *= 2;

            aimTarget.transform.localPosition = mousePos;

            float angle = Vector2.Angle(Vector2.up, mousePos);

            //Debug.Log("Angle: " + angle);

            Vector3 axis = new Vector3(0, 0, 1);

            Vector3 lookAtPos = Vector3.zero;
            lookAtPos.z = aimTarget.transform.position.y;

           // WeaponArm.transform.RotateAround(AimRotationalPivot.position, axis, angle);
            WeaponArm.transform.LookAt(lookAtPos);
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

        if (!isGrounded)
        {
            return;
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
        
        if (other.gameObject.CompareTag("Ground") || other.gameObject.layer.ToString() == "Ground" || other.gameObject.CompareTag("Puddle") || other.gameObject.layer.ToString() == "Puddle")
        {
              
            isGrounded = true;
            Debug.Log("Player is grounded.");
            //anim.SetInteger("State", 0);
            Debug.Log("Stopped jumping.");
            
        }
        else
        {
            isGrounded = false;
            Debug.Log("Player is NOT grounded.");
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
}


