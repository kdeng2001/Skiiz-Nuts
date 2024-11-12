using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerAnimation contains methods for playing player racing animations.
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnimator;
    private SpriteRenderer playerSprite;

    // The names of animations as specified in the PlayerAnimator asset.
    public readonly string IdleTurn = "IdleTurn";
    public readonly string IdleBack = "IdleBack";
    public readonly string AccelerateTurn = "AccelerateTurn";
    public readonly string AccelerateBack = "AccelerateBack";
    public readonly string StartDrifting = "StartDrifting";
    public readonly string Drifting = "Drifting";
    public readonly string EndDrifting = "EndDrifting";

    private bool moveAnimations = true;

    private string currentAnimation = "";
    private bool driftFlip;
    /// <summary>
    /// Gets necessary components for playing animations.
    /// </summary>
    private void Awake()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
    }
    /// <summary>
    /// Enables animations relating to movement, making them playable.
    /// </summary>
    public void EnableMoveAnimations() { moveAnimations = true; }
    /// <summary>
    /// Disables animations relating to movement, making them unplayable.
    /// </summary>
    public void DisableMoveAnimations() { moveAnimations = false; }
    /// <summary>
    /// Returns true if an animation has ended.
    /// </summary>
    /// <param name="animation">The name of an animation as a string. </param>
    /// <returns></returns>
    public bool AnimationHasEnded(string animation)
    {
        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).loop &&
            playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(animation) &&
            playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            // Animation has ended, do something
            return true;
        }
        return false;
    }
    // *** IDLE Animation Methods ***
    /// <summary>
    /// Sets the animation to idle, depending on the direction the player is facing.
    /// </summary>
    public void SetIdle() 
    {
        if(!moveAnimations) { return; }
        if (IsEndingDrift()) { return; }
        if (PlayerActionManager.Instance.moveValue.x > 0) { playerSprite.flipX = true; SetIdleTurn(); }
        else if (PlayerActionManager.Instance.moveValue.x < 0) { playerSprite.flipX = false; SetIdleTurn(); }
        else { SetIdleBack(); }
    }
    /// <summary>
    /// Handles default idle, full back view of player.
    /// </summary>
    public void SetIdleBack() 
    { 
        if(currentAnimation == IdleBack) { return; }
        playerAnimator.Play(IdleBack);
        currentAnimation = IdleBack;
    }
    /// <summary>
    /// Handles 3/4 idle view of player.
    /// </summary>
    public void SetIdleTurn() 
    {
        if (currentAnimation == IdleTurn) { return; }
        playerAnimator.Play(IdleTurn);
        currentAnimation = IdleTurn;
    }
    
    // *** ACCELERATE Animation Methods ***
    /// <summary>
    /// Sets the animation to accelerate, depending on the direction the player is facing.
    /// </summary>
    public void SetAccelerate() 
    {
        if(!moveAnimations) { return; }
        if(IsEndingDrift()) { return; }
        if (PlayerActionManager.Instance.moveValue.x > 0) { playerSprite.flipX = true; SetAccelerateTurn(); }
        else if (PlayerActionManager.Instance.moveValue.x < 0) { playerSprite.flipX = false; SetAccelerateTurn(); }
        else { SetAccelerateBack(); }
    }
    /// <summary>
    /// Handles the default accelerate, full back view of player.
    /// </summary>
    public void SetAccelerateBack() 
    {
        if (currentAnimation == AccelerateBack) { return; }
        playerAnimator.Play(AccelerateBack);
        currentAnimation = AccelerateBack;
    }
    /// <summary>
    /// Handles 3/4 accelerate view of player.
    /// </summary>
    public void SetAccelerateTurn() 
    {
        if (currentAnimation == AccelerateTurn) { return; }
        playerAnimator.Play(AccelerateTurn);
        currentAnimation = AccelerateTurn;
    }
    // *** DRIFT Animation Methods ***
    /// <summary>
    /// Sets the animation for the start of drift, depending on the direction the player is drifting.
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
    /// <summary>
    /// Plays an animation when drifting ends, depending on the direction the player faced when drifting started.
    /// </summary>
    public void SetEndDrift()
    {
        if(currentAnimation == EndDrifting) { return; }
        playerSprite.flipX = driftFlip;
        playerAnimator.Play(EndDrifting);
        currentAnimation = EndDrifting;
    }
    /// <summary>
    /// Plays the animation that plays while continuing drifting.
    /// </summary>
    public void SetDrifting()
    {
        if(currentAnimation == Drifting) { return; }
        playerSprite.flipX = driftFlip;
        playerAnimator.Play(Drifting);
        currentAnimation = Drifting;
    }
    /// <summary>
    /// Checks if the EndDrifting animation is still ongoing.
    /// </summary>
    /// <returns> True if the EndDrifting animation is still playing. False otherwise. </returns>
    public bool IsEndingDrift() { return (currentAnimation == EndDrifting && !AnimationHasEnded(EndDrifting)); }
}
