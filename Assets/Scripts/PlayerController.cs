using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMovement pMovement;
    public Steer pSteer;
    public Drift pDrift;
    public PlayerAnimation pAnimation;
    
    private void Awake()
    {
        pMovement = GetComponent<PlayerMovement>();
        pSteer = GetComponent<Steer>();
        pDrift = GetComponent<Drift>();
        pAnimation = GetComponent<PlayerAnimation>();
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

    void FixedUpdate()
    {
        if (!pDrift.drifting) { pSteer.Steering(PlayerActionManager.Instance.moveValue.x); }
        pDrift.Drifting(PlayerActionManager.Instance.moveValue.x);
        pMovement.Move(pSteer.direction);
        //pAnimation.HandleAnimation();
    }
}
