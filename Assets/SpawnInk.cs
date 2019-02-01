using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInk : MonoBehaviour
{
	public GameObject inkPuddlePrefab;
	public ParticleSystem PS;
	public List<ParticleCollisionEvent> collisionEvents;

	// Use this for initialization
	void Start () {
		
		PS = GetComponent<ParticleSystem>();
		collisionEvents = new List<ParticleCollisionEvent>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnParticleCollision(GameObject other)
	{
		int i = 0;
		int numCollisionEvents = PS.GetCollisionEvents(other, collisionEvents);
		while (i < numCollisionEvents)
		{
			if (other.CompareTag("Ground") || other.gameObject.layer.ToString() == "Ground")
			{
				Vector3 pos = collisionEvents[i].intersection;
				var offsetY = new Vector3(0, -0.6f, 0);
				GameObject newInkPuddle = Instantiate(inkPuddlePrefab, pos + offsetY, Quaternion.identity);
			}
			i++;
		}
	}
	
	
}
