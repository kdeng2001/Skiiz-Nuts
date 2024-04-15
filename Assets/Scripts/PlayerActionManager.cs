using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionManager : MonoBehaviour
{
    public static PlayerActionManager Instance;
    public Vector2 moveValue;
    public bool driftValue;
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
            InputAction driftAction = playerInput.actions["Drift"];
            if(driftAction != null)
            {
                driftAction.performed += context => OnDriftStart(context);
                driftAction.canceled += context => OnDriftEnd(context);
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
            InputAction driftAction = playerInput.actions["Drift"];
            if (driftAction != null)
            {
                driftAction.performed -= context => OnDriftStart(context);
                driftAction.canceled -= context => OnDriftEnd(context);
            }
        }
    }

    public void OnDriftStart(InputAction.CallbackContext context)
    {
        driftValue = context.ReadValueAsButton();
        PlayerEvents.Instance.onStartDrift?.Invoke();
        Debug.Log("action manager drift");
    }

    public void OnDriftEnd(InputAction.CallbackContext context)
    {
        driftValue = context.ReadValueAsButton();
        PlayerEvents.Instance.onEndDrift?.Invoke();
    }
}
