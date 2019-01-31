using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    public Transform AimTransform;
    Transform sPosAimTransform;

    // Use this for initialization
    void Start () {
        sPosAimTransform = AimTransform;
	}
	
	// Update is called once per frame
	void Update () {
        Aiming();
	}

    void Aiming ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, sPosAimTransform.position.z));
            Debug.Log(pos);
            // code
            AimTransform.position = pos;
             }
    }

}
