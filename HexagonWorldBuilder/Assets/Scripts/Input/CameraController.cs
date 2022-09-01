using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementTime;
    
    [Header("Border Sensitivity")]
    [SerializeField] private float borderSensitivity = 100;
    [SerializeField] private RectTransform ignoreBorder;

    [Header("Zoom Properties")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 zoomAmount;
    [SerializeField] private Vector3 minZoom;
    [SerializeField] private Vector3 maxZoom;


    private Vector3 _newPosition;
    private Vector3 _newZoom;
    private float _lastValue = 0;

    private InputManager _inputManager;

    private void Awake()
    {
        _inputManager = InputManager.Instance;
    }

    private void Start()
    {
        _newPosition = transform.position;
        _newZoom = cameraTransform.localPosition;
    }
    
    private void OnEnable()
    {
        _inputManager.OnMouseScroll += HandleZoomInput;
    }
    
    private void OnDisable()
    {
        _inputManager.OnMouseScroll -= HandleZoomInput;
    }

    private void LateUpdate()
    {
        HandleMovementInput(Mouse.current.position.ReadValue());
    }

    private void HandleMovementInput(Vector2 mousePos)
    {
        if (mousePos.x > ignoreBorder.position.x - ignoreBorder.rect.width / 2 &&
            mousePos.y > ignoreBorder.position.y - ignoreBorder.rect.height / 2)
        {
            MoveCamera();
            return;
        }
        if (mousePos.x < borderSensitivity)
        {
            _newPosition += transform.right * -movementSpeed;
        }
        else if (mousePos.x > Screen.width - borderSensitivity)
        {
            _newPosition += transform.right * movementSpeed;
        }
        if (mousePos.y < borderSensitivity)
        {
            _newPosition += transform.forward * -movementSpeed;
        }
        else if (mousePos.y > Screen.height - borderSensitivity)
        {
            _newPosition += transform.forward * movementSpeed;
        }
        
        MoveCamera();
    }

    private void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * movementTime);
    }

    private void HandleZoomInput(float value)
    {
        if (value < 0)
        {
            if (_lastValue > 0)
            {
                _newZoom = cameraTransform.localPosition;
            }
            _newZoom += zoomAmount;
        } else if (value > 0)
        {
            if (_lastValue < 0)
            {
                _newZoom = cameraTransform.localPosition;
            }
            _newZoom -= zoomAmount;
        }

        _lastValue = value;
        
        Vector3 newZoomPosition = Vector3.Lerp(cameraTransform.localPosition, _newZoom, Time.deltaTime * movementTime);
        cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x,
            Mathf.Clamp(newZoomPosition.y, minZoom.y, maxZoom.y), Mathf.Clamp(newZoomPosition.z, maxZoom.z, minZoom.z));
    }
}
