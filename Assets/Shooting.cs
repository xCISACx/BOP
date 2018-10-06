using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

	public GameObject inkPrefab;
	public GameObject player;
	public float timer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		timer += 1.0f * Time.deltaTime;	
		DestroyDistance();
	}

	/*void DestroyAfter()
	{
		if (timer >= 3)
		{
			GameObject.Destroy(inkPrefab);
		}
		
	}*/

	void DestroyDistance()
	{
		float distance = Vector2.Distance(inkPrefab.transform.position, player.transform.position);

		if (distance > 3)
			//inkPrefab.SetActive(false);
			Destroy(inkPrefab);
	}
}
