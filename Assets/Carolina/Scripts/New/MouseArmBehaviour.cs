using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Description("Class for horizontally flipping a model and rotating an arm based on mouse position")]
public class MouseArmBehaviour : MonoBehaviour
{

    [Header("Camera for Mouse Ray Casting")]
    public Camera camera;

    [Header("Arm Pivot Transform to Rotate")]
    public Transform armPivot;

    private bool _flipped = false;

    private float _initalScaleX;
	// Use this for initialization
	void Start ()
    {
        _initalScaleX = transform.localScale.x;

    }
	
	// Update is called once per frame
	void Update ()
    {
        #region Variables Declaration

        Vector2 mousePos = Input.mousePosition;
        var worldPos = camera.ScreenToWorldPoint(mousePos);
        
        #endregion

        #region CheckAssetFacingDirection

        if (worldPos.x < transform.position.x && !_flipped)
        {
            _flipped = true;
        }
        if (worldPos.x > transform.position.x && _flipped)
        {
            _flipped = false;
        }

        if (_flipped)
        {
            transform.localScale = new Vector3(-_initalScaleX, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(_initalScaleX, transform.localScale.y, transform.localScale.z);
        }

        #endregion

        #region ArmRotationDirection

        armPivot.LookAt(new Vector3(worldPos.x,worldPos.y, armPivot.position.z));

        #endregion
    }
}
