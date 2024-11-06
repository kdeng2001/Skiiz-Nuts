using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// PlayerActionManager is a singleton storing player input events as variables
/// to be referenced by other scripts.
/// </summary>
public class PlayerActionManager : MonoBehaviour
{
    public static PlayerActionManager Instance;
    public PlayerEvents playerEvents;
    public Vector2 moveValue;
    public bool driftValue;

    /// <summary>
    /// Creates a PlayerEvents component and adds it to the Player Sprite.
    /// Ensures only a single instance of PlayerActionManager exists.
    /// </summary>
    private void Awake()
    {
        if(Instance != null && Instance != this) { Destroy(this); return; }
        else { Instance = this; }
        playerEvents = gameObject.AddComponent<PlayerEvents>();
    }

    /// <summary>
    /// Subscribes input events to a function that sets the PlayerActionManager's properties to the input values.
    /// </summary>
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

    /// <summary>
    /// Unsubscribes input events from a function that sets the PlayerActionManager's properties to the input values.
    /// </summary>
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

    /// <summary>
    /// Invokes an event from playerEvents when drifting starts.
    /// </summary>
    /// <param name="context"> Contains information on actions. </param>
    public void OnDriftStart(InputAction.CallbackContext context)
    {
        driftValue = context.ReadValueAsButton();
        playerEvents.onStartDrift?.Invoke(moveValue.x);
    }

    /// <summary>
    /// Invokes an event from playerEvents when drifting ends.
    /// </summary>
    /// <param name="context"> Contains information on actions. </param>
    public void OnDriftEnd(InputAction.CallbackContext context)
    {
        driftValue = context.ReadValueAsButton();
        playerEvents.onEndDrift?.Invoke();
    }
}
