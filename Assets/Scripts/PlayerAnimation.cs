using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator playerAnimator;
    SpriteRenderer playerSprite;

    public readonly string IdleTurn = "IdleTurn";
    public readonly string IdleBack = "IdleBack";
    public readonly string AccelerateTurn = "AccelerateTurn";
    public readonly string AccelerateBack = "AccelerateBack";
    public readonly string StartDrifting = "StartDrifting";
    public readonly string Drifting = "Drifting";
    public readonly string EndDrifting = "EndDrifting";

    bool moveAnimations = true;

    string currentAnimation = "";
    bool driftFlip;
    private void Awake()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
    }
    public void EnableMoveAnimations() { moveAnimations = true; }
    public void DisableMoveAnimations() { moveAnimations = false; }

    public bool AnimationHasEnded(string animation)
    {
        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).loop &&
            playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(animation) &&
            playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            // Animation has ended, do something
            Debug.Log("Animation ended!");
            return true;
        }
        return false;
    }
    /// <summary>
    /// Idle animation functions
    /// </summary>
    public void SetIdle() 
    {
        if(!moveAnimations) { return; }
        if (currentAnimation == EndDrifting && !AnimationHasEnded(EndDrifting)) { return; }
        if (PlayerActionManager.Instance.moveValue.x > 0) { playerSprite.flipX = true; SetIdleTurn(); }
        else if (PlayerActionManager.Instance.moveValue.x < 0) { playerSprite.flipX = false; SetIdleTurn(); }
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
    public void SetAccelerate() 
    {
        if(!moveAnimations) { return; }
        if (currentAnimation == EndDrifting && !AnimationHasEnded(EndDrifting)) { return; }
        if (PlayerActionManager.Instance.moveValue.x > 0) { playerSprite.flipX = true; SetAccelerateTurn(); }
        else if (PlayerActionManager.Instance.moveValue.x < 0) { playerSprite.flipX = false; SetAccelerateTurn(); }
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
    
    /// <summary>
    /// Drift animation functions
    /// </summary>
    public void SetStartDrift()
    {
        if(currentAnimation == StartDrifting) { return; }
        if (PlayerActionManager.Instance.moveValue.x > 0) { driftFlip = playerSprite.flipX = true; }
        else if(PlayerActionManager.Instance.moveValue.x < 0) { driftFlip = playerSprite.flipX = false; }
        else { return; }
        playerAnimator.Play(StartDrifting);
        currentAnimation = StartDrifting;
    }
    public void SetEndDrift()
    {
        if(currentAnimation == EndDrifting) { return; }
        playerSprite.flipX = driftFlip;
        playerAnimator.Play(EndDrifting);
        currentAnimation = EndDrifting;
    }
    public void SetDrifting()
    {
        if(currentAnimation == Drifting) { return; }
        playerSprite.flipX = driftFlip;
        playerAnimator.Play(Drifting);
        currentAnimation = Drifting;
    }
}
