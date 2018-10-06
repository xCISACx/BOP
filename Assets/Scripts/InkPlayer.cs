using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkPlayer : MonoBehaviour {

	public float fireRate = 0;
	public float damage = 1;
	public LayerMask notToHit;
	float timeToFire = 0;
	Transform firePoint;

	// Use this for initialization
	void Awake () 
	{

		firePoint = transform.Find("PlayerFirePoint");
		if (firePoint == null)
		{
			Debug.LogError("No firePoint set! Why???");
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Destroy(gameObject);
	}

	// Update is called once per frame
	void Update () {
	}

}
