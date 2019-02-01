using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
	[Header("Stats")]
	public float enemyHP = 2;
	
	[Header("Scripts")]
	
	public SpawnInk spawnInk;

	public SpawnAmmo spawnAmmo;

	[Header("Conditions")]
	
	public bool canBeDamaged = true;

	[Header("Timers")]
	
	public float invulTime;

	public EnemySpawner spawner;

	public Texture2D HPTickTexture;

	public SpawnInk.EnemyType type;

	// Use this for initialization
	void Start ()
	{
		spawnInk = GameObject.Find("InkSpray").GetComponent<SpawnInk>();
		spawnAmmo = GetComponent<SpawnAmmo>();
		if (spawnAmmo == null)
		{
			Debug.Log("Couldn't find spawn ammo script");
		}
		ChangeColour(type);
	}

	private void Update()
	{
		spawnInk = GameObject.Find("InkSpray").GetComponent<SpawnInk>();
		spawnAmmo = GetComponent<SpawnAmmo>();
		if (spawnAmmo == null)
		{
			Debug.Log("Couldn't find spawn ammo script");
		}
		ChangeColour(type);
		Death();
	}

	// Update is called once per frame
	/*void LateUpdate () 
	{
		Type = spawnInk.enemyType;
	}*/

	void ChangeColour(SpawnInk.EnemyType Type)
	{
		switch (Type)
		{
			case SpawnInk.EnemyType.Bouncy:
				gameObject.transform.GetComponent<SpriteRenderer>().color = spawnInk.BouncyColour;
				break;
			case SpawnInk.EnemyType.Speedy:
				gameObject.transform.GetComponent<SpriteRenderer>().color = spawnInk.SpeedyColour;
				break;
			case SpawnInk.EnemyType.Sticky:
				gameObject.transform.GetComponent<SpriteRenderer>().color = spawnInk.StickyColour;
				break;
			case SpawnInk.EnemyType.Clear:
				gameObject.transform.GetComponent<SpriteRenderer>().color = spawnInk.ClearColour;
				break;
		}
	}

	public void ApplyDamage(float dmg)
	{
		if (canBeDamaged)
		{
			enemyHP -= dmg;
			StartCoroutine(JustDamaged());
		}
	}
						
	IEnumerator JustDamaged()
	{
		canBeDamaged = false;
		yield return new WaitForSeconds(invulTime);
		canBeDamaged = true;
	}

	public void Death()
	{
		if (enemyHP <= 0)
		{
			spawnAmmo.TypeCheck();
			spawner.StartCooldownTimer();
			Destroy(gameObject);
		}
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Acid"))
		{
			Destroy(gameObject);
		}
	}
	
	/*private void OnGUI()
	{
		GUI.DrawTexture(new Rect(gameObject.transform.position.x,gameObject.transform.position.y + 20,16,16), HPTickTexture);
	}*/
}
