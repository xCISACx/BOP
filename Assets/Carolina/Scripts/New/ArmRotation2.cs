using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class ArmRotation2 : MonoBehaviour
{
	public float minArmLock;
	public float maxArmLock;
	public float minArmLockInv;
	public float maxArmLockInv;
	public int rotationOffset = 90;
	[SerializeField] GameObject thePlayer;
	Vector3 playerScale;

	// Use this for initialization
	void Start () {
		
		//armDefaultPos = Vector
		playerScale = thePlayer.transform.lossyScale;
	}
	
	// Update is called once per frame
	void Update () 
	{

		Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		difference.Normalize();

		float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
	}
}
