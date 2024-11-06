using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerController handles high level player racing mechanics and 
/// stores references to all components relating to player racing mechanics.
/// These references are publicly accessible to other scripts.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public PlayerMovement pMovement;
    public Steer pSteer;
    public Drift pDrift;
    public Grounding ground;
    public PlayerAnimation pAnimation;
    public SpriteRenderer sprite;
    public SphereCollider body;
    public Rigidbody rb;

    /// <summary>
    /// Finds corresponding components for each property.
    /// </summary>
    private void Awake()
    {
        pMovement = GetComponent<PlayerMovement>();
        pSteer = GetComponent<Steer>();
        pDrift = GetComponent<Drift>();
        ground = GetComponent<Grounding>();
        pAnimation = GetComponent<PlayerAnimation>();
        sprite = GetComponent<SpriteRenderer>();
        body = transform.parent.GetComponentInChildren<SphereCollider>();
        rb = transform.parent.GetComponentInChildren<Rigidbody>();
    }

    /// <summary>
    /// Subscribes pDrift’s StartDrift() and EndDrift() functions to the corresponding delegates in playerEvents.
    /// </summary>
    private void OnEnable()
    {
        PlayerActionManager.Instance.playerEvents.onStartDrift += pDrift.StartDrift;
        PlayerActionManager.Instance.playerEvents.onEndDrift += pDrift.EndDrift;
    }

    /// <summary>
    /// Unsubscribes pDrift’s StartDrift() and EndDrift() functions from the corresponding delegates in playerEvents.
    /// </summary>
    private void OnDisable()
    {
        PlayerActionManager.Instance.playerEvents.onStartDrift -= pDrift.StartDrift;
        PlayerActionManager.Instance.playerEvents.onEndDrift -= pDrift.EndDrift;
    }

    /// <summary>
    /// Ensures the sprite property’s transform is in the same position as the rb, which moves due to physics.
    /// </summary>
    private void Update()
    {
        sprite.transform.position = body.transform.position;
    }

    /// <summary>
    /// Determines whether to drift or steer based on input, 
    /// and attempts to move the player regardless of whether the player is drifting or steering.
    /// </summary>
    private void FixedUpdate()
    {
        if (!pDrift.drifting) { pSteer.Steering(PlayerActionManager.Instance.moveValue.x); }
        { pDrift.Drifting(PlayerActionManager.Instance.moveValue.x); }
        pMovement.Move(pSteer.newDirection);
    }
}
