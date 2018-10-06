using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerArrows : MonoBehaviour {

	public Transform firePoint;
	public LayerMask notToHit;
	public GameObject inkPrefab;
	public GameObject meh;
	public GameObject ink;
	public float shootingSpeed = 1;
	public Vector2 velocity;

    //definir a velocidade do salto
    //variaveis publicas são visiveis dentro do painel inspector
    public float speedX;
    public float jumpSpeedY;
	public float speedhacks;

    //declaramos as variáveis a usar
    //variaveis privadas não são visíveis dentro do inspector, quando não diz nada, é privado
    Animator anim;
    Rigidbody2D rb;

    bool facingRight, Jumping;
    float speed;

    [SerializeField]
    private PolygonCollider2D[] colliders;
    private int currentColliderIndex = 0;

    // Use this for initialization
    void Start()
    {
        //estamos a fazer uma referência a esse componente a partir do nome que definimos
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        facingRight = true;

    }

    // Update is called once per frame
    void Update()
    {
        //chama o método para o movimento do jogador
        MovePlayer(speed);
        Flip();
        //when the player lets go off the right or left arrow, stop moving.
        if ( (Input.GetKeyUp(KeyCode.RightArrow) || (Input.GetKeyUp(KeyCode.LeftArrow))) )
        {
            anim.SetInteger("State", 0);
            speed = 0;
        }
		if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            speed = -speedX;
            anim.SetInteger("State", 1);
        }
        //movimento jogador para direita
		if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            anim.SetInteger("State", 1);
            speed = speedX;
        }
        //movimento jogador saltar
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jumping = true;
            rb.AddForce(new Vector2(rb.velocity.x, jumpSpeedY));
            anim.SetInteger("State", 12);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            anim.SetInteger("State", 0);
            speed = 0;
        }
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			speed = 0;
			anim.SetInteger("State", 2);
		}
		if (Input.GetKeyUp(KeyCode.	DownArrow))
		{
			speed = 0;
			anim.SetInteger ("State", 10);
		}
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			//anim.SetInteger("State", 3);
			ink.GetComponent<ParticleSystem>().enableEmission = true;
			ink.GetComponent<ParticleSystem>().Play();
			Shoot();
		}
	    
		if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			ink.GetComponent<ParticleSystem>().enableEmission = false;
			//anim.SetInteger("State", 0);
		}
	    
	    if (Input.GetKeyDown(KeyCode.LeftShift))
	    {
		    speed *= speedhacks;
	    }
    }
    // cria a função MovePlayer
    void MovePlayer(float playerSpeed)
    {
        //define o método do movimento do jogador
        // "&&" = e "||" = ou 
        // se o jogador se estiver a mover esquerda ou direita ou a saltar vai para o State1 - run
        if (playerSpeed < 0 && !Jumping || playerSpeed > 0 && !Jumping)
        {
            anim.SetInteger("State", 1);
        }
        // se o jogador se estiver a mover esquerda ou direita ou a saltar vai para o State0  - idle
        if (playerSpeed < 0 && !Jumping)
        {
        }
         rb.velocity = new Vector2(speed, rb.velocity.y);
    }
    //fazer flip ao sprite do jogador
    void Flip()
    {
        if (speed > 0 && !facingRight || speed < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 temp = transform.localScale;
            temp.x *= -1;
            transform.localScale = temp;
        }
    }

    /*public void SetColliderForSprite( int spriteNum )
    {
		//Debug.Log("switching from " + currentColliderIndex + " to " + spriteNum);
        colliders[currentColliderIndex].enabled = false;
        currentColliderIndex = spriteNum;
        colliders[currentColliderIndex].enabled = true;
    }*/

	/*public void EndOfSpawnAnim()
	{
		anim.SetInteger("State", 0);
	}*/

	public void Shoot()
	{
		Debug.Log("Shooting");
		firePoint = transform.Find("PlayerFirePoint");
		if (firePoint != null)
		{
			Debug.Log("firePoint set!");
		}

		ink.GetComponent<ParticleSystem>().enableEmission = true;
		inkPrefab = Instantiate(ink, firePoint.position, firePoint.rotation);
		inkPrefab.tag = "ink";
		//GameObject ink = (GameObject)Instantiate(meh,(Vector2)firePoint.transform.position * transform.localScale.x, Quaternion.identity);
		Debug.Log("Coordinates:" + firePoint.transform.position.x + ", " + firePoint.transform.position.y);
		//ink.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y);
		Destroy(GameObject.FindWithTag("ink"), 5);
	}
}

