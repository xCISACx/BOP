using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Animator anim;
    public float horAxis;
    public float verAxis;


    Vector3 playerScale;

    void Start () 
    {
        playerScale = thePlayer.transform.lossyScale;
        anim = GetComponent<Animator>();
    }
	
	void Update () 
	{
        Aiming();       //  TODO : Move to FixedUpdate
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
            
        }

        
    }

    void Aiming ()
    {
        if (Input.GetMouseButtonDown(0)) // Bool : Has weapon?
        {

            Vector2 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            mousePos.x = mousePos.x - 0.5f;
            mousePos.y = mousePos.y - 0.5f;

            mousePos *= 2;

            aimTarget.transform.localPosition = mousePos;

            float angle = Vector2.Angle(Vector2.up, mousePos);

            Debug.Log("Angle: " + angle);

            Vector3 axis = new Vector3(0, 0, 1);

            Vector3 lookAtPos = Vector3.zero;
            lookAtPos.z = aimTarget.transform.position.y;

           // WeaponArm.transform.RotateAround(AimRotationalPivot.position, axis, angle);
           WeaponArm.transform.LookAt(lookAtPos);

        }
    }

}


