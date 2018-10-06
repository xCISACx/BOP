using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ink : MonoBehaviour {

    public float fireRate = 0;
    public float damage = 1;
    public LayerMask notToHit;
    float timeToFire = 0;
    Transform firePoint;

	// Use this for initialization
	void Awake () {

        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No firePoint set! Why???");
        }
    }
	
	// Update is called once per frame
	void Update () {
	}

    }
