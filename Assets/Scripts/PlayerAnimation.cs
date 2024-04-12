using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator playerAnimator;
    SpriteRenderer playerSprite;

    static string IdleTurn = "IdleTurn";
    static string IdleBack = "IdleBack";
    static string AccelerateTurn = "AccelerateTurn";
    static string AccelerateBack = "AccelerateBack";


    string currentAnimation = "";
    private void Awake()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void HandleAnimation(int angle) 
    {
        //Debug.Log(angle);
        if(PlayerActionManager.Instance.moveValue.y > 0) { SetAccelerate(angle); }
        else { SetIdle(angle); }
    }

    /// <summary>
    /// Idle animation functions
    /// </summary>
    public void SetIdle(int angle) 
    {
        if(angle > 20) { playerSprite.flipX = true; SetIdleTurn(); }
        else if (angle < -20) { playerSprite.flipX = false; SetIdleTurn(); }
        else { SetIdleBack(); }
    }
    public void SetIdleBack() 
    { 
        if(currentAnimation == IdleBack) { return; }
        playerAnimator.Play(IdleBack);
        currentAnimation = IdleBack;
    }
    public void SetIdleTurn() 
    {
        if (currentAnimation == IdleTurn) { return; }
        playerAnimator.Play(IdleTurn);
        currentAnimation = IdleTurn;
    }
    
    /// <summary>
    /// Accelerate animation functions
    /// </summary>
    public void SetAccelerate(int angle) 
    {
        if (angle > 20) { playerSprite.flipX = true; SetAccelerateTurn(); }
        else if (angle < -20) { playerSprite.flipX = false; SetAccelerateTurn(); }
        else { SetAccelerateBack(); }
    }
    public void SetAccelerateBack() 
    {
        if (currentAnimation == AccelerateBack) { return; }
        playerAnimator.Play(AccelerateBack);
        currentAnimation = AccelerateBack;
    }
    public void SetAccelerateTurn() 
    {
        if (currentAnimation == AccelerateTurn) { return; }
        playerAnimator.Play(AccelerateTurn);
        currentAnimation = AccelerateTurn;
    }
    
}
