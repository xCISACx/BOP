﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class ArmRotation : MonoBehaviour
{

	public int rotationOffset = 90;

	// Use this for initialization
	void Start () {
		
		//armDefaultPos = Vector
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton(1))
		{
			Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
			difference.Normalize();

			float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
		}

	}
}
