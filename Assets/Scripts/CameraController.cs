using System;
using Data;
using UnityEngine;

public class CameraController:MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float zoomSpeed = 2;
    [SerializeField] private float minCameraSize = GameSettings.ZOOM_MIN;
    [SerializeField] private float maxCameraSize = GameSettings.ZOOM_MAX;
    private bool _mouseMove = SaveDataStorage.MOUSE_NAVIGATION_ENABLED;
    private bool _keyboardMove = SaveDataStorage.KEYBOARD_NAVIGATION_ENABLED;

    private Camera _camera;
    private readonly float _borderMargin = 0.025f;

    private float xStart;
    private float yStart;

    public void SetCameraStartCoordinates()
    {
        xStart = _camera.transform.position.x;
        yStart = _camera.transform.position.y;
    }

    private void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
        SetCameraStartCoordinates();
    }

    private void Update()
    {
        if(_mouseMove) MouseMove();
        if (_keyboardMove) KeyboardMove();
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
        
        float x = mousePosition.x;
        float y = mousePosition.y;
        if (x < 0 || y < 0 || x > 1 || y > 1) return;
        
        float xInput = mousePosition.x > 0.5f ? 1 :-1;
        float yInput = mousePosition.y > 0.5f ? 1 :-1;
        
        if (x < _borderMargin || 1 - x < _borderMargin)
            Move(xInput, 0);
        if(y < _borderMargin || 1 - y < _borderMargin)
            Move(0, yInput);
    }

    private void Move(float x, float y)
    {
        Vector3 direction = transform.right * x + transform.up * y;
        var position = transform.position;
        position += direction * moveSpeed * Time.deltaTime * _camera.orthographicSize/5;

        if (Math.Abs(xStart - position.x) > 20 || Math.Abs(yStart - position.y) > 20)
            return;
        
        transform.position = position;
    }

    private void Zoom()
    {
        float scrollInput = Input.mouseScrollDelta.y;
        if(scrollInput == 0) return;

        float size =_camera.orthographicSize - zoomSpeed * scrollInput;
        if (size < minCameraSize || size > maxCameraSize) return;

        _camera.orthographicSize = size;
    }
}