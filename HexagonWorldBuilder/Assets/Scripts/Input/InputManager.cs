using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    private InputControls _inputControls;

    public delegate void RightMouseClickEvent(Vector2 clickPosition);
    public event RightMouseClickEvent OnRightMouseClick;
    
    public delegate void LeftMouseClickEvent(Vector2 clickPosition);
    public event LeftMouseClickEvent OnLeftMouseClick;
    
    public delegate void MouseScrollEvent(float value);
    public event MouseScrollEvent OnMouseScroll;

    private void Awake()
    {
        _inputControls = new InputControls();
    }

    private void OnEnable()
    {
        _inputControls.Enable();
    }

    private void OnDisable()
    {
        _inputControls.Disable();
    }

    private void Start()
    {
        _inputControls.Mouse.RightClick.started += _ => RightMouseClick();
        _inputControls.Mouse.LeftClick.started += _ => LeftMouseClick();
        _inputControls.Mouse.Scroll.performed += ctx => MouseScroll(ctx);
        _inputControls.Mouse.Quit.started += _ => QuitGame();
    }

    private void RightMouseClick()
    {
        Vector2 clickPosition = Mouse.current.position.ReadValue();

        if (OnRightMouseClick != null)
        {
            OnRightMouseClick(clickPosition);
        }
    }
    
    private void LeftMouseClick()
    {
        Vector2 clickPosition = Mouse.current.position.ReadValue();

        if (OnLeftMouseClick != null)
        {
            OnLeftMouseClick(clickPosition);
        }
    }

    private void MouseScroll(InputAction.CallbackContext ctx)
    {
        float value = ctx.ReadValue<float>();

        if (OnMouseScroll != null)
        {
            OnMouseScroll(value);
        }
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
