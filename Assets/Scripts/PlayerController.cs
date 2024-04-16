using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMovement pMovement;
    public Steer pSteer;
    public PlayerDrift pDrift;
    public PlayerAnimation pAnimation;
    
    private void Awake()
    {
        pMovement = GetComponent<PlayerMovement>();
        pSteer = GetComponent<Steer>();
        pDrift = GetComponent<PlayerDrift>();
        pAnimation = GetComponent<PlayerAnimation>();
    }
    void FixedUpdate()
    {
        pSteer.Steering(PlayerActionManager.Instance.moveValue.x);
        pDrift.Drift();
        pMovement.Move(pSteer.direction);
        //pAnimation.HandleAnimation();
    }
}
