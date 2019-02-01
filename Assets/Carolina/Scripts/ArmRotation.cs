using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class ArmRotation : MonoBehaviour
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

		if (difference.x > 0)
			thePlayer.transform.localScale = new Vector3(playerScale.x,playerScale.y,playerScale.z);
		if (difference.x < 0)
			thePlayer.transform.localScale = new Vector3(-playerScale.x,playerScale.y,playerScale.z);
		//Debug.Log("Arm Angle: " + transform.localEulerAngles.z);
		if (difference.x > 0)
			transform.localEulerAngles = new Vector3(0, 0, Mathf.Clamp(transform.localEulerAngles.z, minArmLock, maxArmLock));
		else
		{
			transform.localEulerAngles = new Vector3(0, 0, Mathf.Clamp(transform.localEulerAngles.z, minArmLockInv - 90, maxArmLockInv - 90));
		}

		
	}
}
