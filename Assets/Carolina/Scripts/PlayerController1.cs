using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour {

    //definir a velocidade do salto
    //variaveis publicas são visiveis dentro do painel inspector
    public float speedX;
    public float jumpSpeedY;

    //declaramos as variáveis a usar
    //variaveis privadas não são visíveis dentro do inspector, quando não diz nada, é privado
    Animator anim;
    Rigidbody2D rb;

    bool facingRight, Jumping;
    float speed;

    // Use this for initialization
    void Start()
    {
        //estamos a fazer uma referência a esse componente a partir do nome que definimos
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        facingRight = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //chama o método para o movimento do jogador
        MovePlayer(speed);
        Flip();
        //movimento jogador para esquerda
        if ( (Input.GetKeyUp(KeyCode.D) || (Input.GetKeyUp(KeyCode.A) )))
        {
            //anim.SetInteger("State", 0);
            speed = 0;
        }
		if (Input.GetKeyDown(KeyCode.A))
        {
            speed = -speedX;
            //anim.SetInteger("State", 1);
        }
        //movimento jogador para direita
		if (Input.GetKeyDown(KeyCode.D))
        {
            //anim.SetInteger("State", 1);
            speed = speedX;
        }
        //movimento jogador saltar
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jumping = true;
            rb.AddForce(new Vector2(rb.velocity.x, jumpSpeedY));
            //anim.SetInteger("State", 10);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            //anim.SetInteger("State", 0);
            speed = 0;
        }
		if (Input.GetKeyDown(KeyCode.S))
		{
			speed = 0;
			//anim.SetInteger("State", 2);
		}
		if (Input.GetKeyUp(KeyCode.S))
		{
			speed = 0;
			//anim.SetInteger ("State", 10);
		}
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			speed *= 2;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			speed /= 2;
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
            ////anim.SetInteger("State", 1);
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
    //define que quando o jogador volta a tocar no chão deixa de fazer animação de salto (neste caso, o state 0)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PLATAFORMA")
        {
            Jumping = false;
            //anim.SetInteger("State", 0);
        }
    }

}
