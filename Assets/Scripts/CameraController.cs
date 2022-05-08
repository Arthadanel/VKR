using System;
using UnityEngine;

public class CameraController:MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float zoomSpeed = 2;
    [SerializeField] private float minCameraSize = GameSettings.ZOOM_MIN;
    [SerializeField] private float maxCameraSize = GameSettings.ZOOM_MAX;
    [SerializeField] private bool mouseMove = GameSettings.MOUSE_NAVIGATION_ENABLED;
    [SerializeField] private bool keyboardMove = GameSettings.KEYBOARD_NAVIGATION_ENABLED;

    private Camera _camera;
    private float _screenWidth;
    private float _screenHeight;
    private float _borderMultiplier = 0.05f;
    private float _borderW;
    private float _borderH;
    private float _border;

    private void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
        _screenHeight = _camera.pixelHeight;
        _screenWidth = _camera.pixelWidth;
        _borderH = _borderMultiplier * _screenHeight;
        _borderW = _borderMultiplier * _screenWidth;
        _border = 25;
    }

    private void Update()
    {
        if(mouseMove) MouseMove();
        if (keyboardMove) KeyboardMove();
        Zoom();
    }

    private void KeyboardMove()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Move(xInput, yInput);
    }
    
    private void MouseMove()
    {
        Vector3 mousePosition = _camera.ScreenToViewportPoint(Input.mousePosition);
        Debug.Log(_camera.ScreenToViewportPoint(Input.mousePosition));
        
        float x = mousePosition.x;
        float y = mousePosition.y;
        
        if (x < 0 || y < 0 || x > 1 || y > 1) return;
        
        // float xInput = _borderMultiplier / (mousePosition.x > 0.5f ? 1 - mousePosition.x : -mousePosition.x) * 0.2f;
        // float yInput = _borderMultiplier / (mousePosition.y > 0.5f ?1 - mousePosition.y : -mousePosition.y)* 0.2f;
        
        float xInput = mousePosition.x > 0.5f ? 1 :-1;
        float yInput = mousePosition.y > 0.5f ? 1 :-1;
        
        if (x < _borderMultiplier || 1 - x < _borderMultiplier)
            Move(xInput, 0);
        if(y < _borderMultiplier || 1 - y < _borderMultiplier)
            Move(0, yInput);
    }

    private void Move(float x, float y)
    {
        Vector3 direction = transform.right * x + transform.up * y;
        transform.position += direction * moveSpeed * Time.deltaTime * _camera.orthographicSize/5;
    }

    private void Zoom()
    {
        float scrollInput = Input.mouseScrollDelta.y;
        if(scrollInput == 0) return;
        //float distance = Vector3.Distance(transform.position, _camera.transform.position);

        float size =_camera.orthographicSize - zoomSpeed * scrollInput;

        if (size < minCameraSize || size > maxCameraSize) return;

        _camera.orthographicSize = size;

    }

    public void FocusOnPosition(Vector3 position)
    {
        transform.position = position;
    }
}