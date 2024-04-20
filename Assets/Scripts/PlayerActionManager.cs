using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionManager : MonoBehaviour
{
    public static PlayerActionManager Instance;
    public PlayerEvents playerEvents;
    public Vector2 moveValue;
    public bool driftValue;
    private void Awake()
    {
        if(Instance != null && Instance != this) { Destroy(this); return; }
        else { Instance = this; }
        playerEvents = gameObject.AddComponent<PlayerEvents>();
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
            InputAction debugPause = playerInput.actions["DebugPause"];
            if (debugPause != null)
            {
                debugPause.performed += context => Debug.Break();
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
            InputAction debugPause = playerInput.actions["DebugPause"];
            if (debugPause != null)
            {
                debugPause.performed -= context => Debug.Break();
            }
        }
    }

    public void OnDriftStart(InputAction.CallbackContext context)
    {
        driftValue = context.ReadValueAsButton();
        playerEvents.onStartDrift?.Invoke(moveValue.x);
        //Debug.Log("action manager drift");
    }

    public void OnDriftEnd(InputAction.CallbackContext context)
    {
        driftValue = context.ReadValueAsButton();
        playerEvents.onEndDrift?.Invoke();
    }
}
