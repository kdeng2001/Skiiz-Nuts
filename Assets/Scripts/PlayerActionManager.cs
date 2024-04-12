using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionManager : MonoBehaviour
{
    public static PlayerActionManager Instance;
    public Vector2 moveValue;
    private void Awake()
    {
        if(Instance != null) { Destroy(this); }
        else { Instance = this; }
    }
    private void OnEnable()
    {
        if(TryGetComponent(out PlayerInput playerInput))
        {
            InputAction moveAction = playerInput.actions["Movement"];
            if(moveAction != null)
            {
                moveAction.performed += context => moveValue = context.ReadValue<Vector2>();
                moveAction.canceled += context => moveValue = context.ReadValue<Vector2>();
            }
        }
    }
    private void OnDisable()
    {
        if (TryGetComponent(out PlayerInput playerInput))
        {
            InputAction moveAction = playerInput.actions["Movement"];
            if (moveAction != null)
            {
                moveAction.performed -= context => moveValue = context.ReadValue<Vector2>();
                moveAction.canceled -= context => moveValue = context.ReadValue<Vector2>();
            }
        }
    }
}
