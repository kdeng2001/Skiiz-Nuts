using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    bool testStart = true;
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
    private void OnEnable()
    {
        PlayerActionManager.Instance.playerEvents.onStartDrift += pDrift.StartDrift;
        PlayerActionManager.Instance.playerEvents.onEndDrift += pDrift.EndDrift;
    }

    private void OnDisable()
    {
        PlayerActionManager.Instance.playerEvents.onStartDrift -= pDrift.StartDrift;
        PlayerActionManager.Instance.playerEvents.onEndDrift -= pDrift.EndDrift;
    }
    private void Update()
    {
        sprite.transform.position = body.transform.position;
    }
    void FixedUpdate()
    {
        if (!pDrift.drifting) { pSteer.Steering(PlayerActionManager.Instance.moveValue.x); }
        /*if (ground.IsGrounded()) */{ pDrift.Drifting(PlayerActionManager.Instance.moveValue.x); }
        pMovement.Move(pSteer.newDirection);
    }
}
