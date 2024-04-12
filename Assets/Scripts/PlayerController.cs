using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMovement pMovement;
    public PlayerRotate pRotate;
    public PlayerAnimation pAnimation;
    
    private void Awake()
    {
        pMovement = GetComponent<PlayerMovement>();
        pRotate = GetComponent<PlayerRotate>();
        pAnimation = GetComponent<PlayerAnimation>();
    }
    void FixedUpdate()
    {
        pRotate.Rotate();
        pMovement.Move(pRotate.direction);
        Debug.Log(pRotate.currentAngle);
        pAnimation.HandleAnimation(pRotate.currentAngle);
    }
}
