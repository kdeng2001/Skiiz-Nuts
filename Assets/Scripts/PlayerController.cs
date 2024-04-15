using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMovement pMovement;
    public PlayerRotate pRotate;
    public PlayerDrift pDrift;
    public PlayerAnimation pAnimation;
    
    private void Awake()
    {
        pMovement = GetComponent<PlayerMovement>();
        pRotate = GetComponent<PlayerRotate>();
        pDrift = GetComponent<PlayerDrift>();
        pAnimation = GetComponent<PlayerAnimation>();
    }
    void FixedUpdate()
    {
        pRotate.Rotate();
        pDrift.Drift();
        pMovement.Move(pRotate.direction);
        //pAnimation.HandleAnimation();
    }
}
