using UnityEngine;
using UnityEngine.UI;

internal class CursorBehaviour : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Image _activeCursor;

    [SerializeField] private Transform _weaponArm;
    
    [SerializeField] private Sprite _defaultCursor;
    [SerializeField] private Sprite _forbiddenCursor;

    private void Start()
    {
        
    }
    
    private void Update()
    {
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        Cursor.visible = false;
        var armPosition = _weaponArm.position;
        var mousePosition = Input.mousePosition;
        
        // Fire a raycast from the arm towards the position of the mouse.
        var rayDirection = mousePosition - _mainCamera.WorldToScreenPoint(armPosition);
        var rayHit = Physics2D.Raycast(armPosition, rayDirection, 10, LayerMask.GetMask("Forbidden"));
        Debug.DrawRay(armPosition, rayDirection, Color.red);
        
        //Debug.Log(rayHit.transform);
        //Debug.Log(transform.localEulerAngles);
        
        // Switch the sprite of the cursor depending on if something was hit or not.
        _activeCursor.sprite = rayHit.transform == null ? _defaultCursor : _forbiddenCursor;
        
        // Hacky fix for setting the color.
        _activeCursor.color = _activeCursor.sprite == _forbiddenCursor ? Color.red : Color.white;
        
        // Update the position of the cursor.
        _activeCursor.transform.position = mousePosition;
    }
}