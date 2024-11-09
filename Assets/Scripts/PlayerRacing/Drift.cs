using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the player's drifting functionality and exposes variables
/// in editor for adjusting varying steer rate when drifting with different inputs.
/// </summary>
public class Drift : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private Steer steer;
    private Rigidbody rb;
    private PlayerMovement playerMovement;
    public bool drifting { get; private set; }
    public bool finishedDrifting { get; private set; }
    public float startDriftTime { get; private set; }
    public int driftInitDirection { get; private set; }
    [Tooltip("Multiplier that decreases acceleration and maxBaseSpeed (from ordinary movement) while drifting.")]
    [SerializeField] private float driftSlowRate = .75f;
    
    [Tooltip("Rate the player steers when drifting while holding the same direction the drift was initiated.")]
    [SerializeField] public float sharpSteerRate = 4;
    [Tooltip("Rate the player steers when drifting and releasing any direction key.")]
    [SerializeField] public float normalSteerRate = 2;
    [Tooltip("Rate the player steers when drifting while holding the opposite direction the drift was initiated.")]
    [SerializeField] public float wideSteerRate = 1;
    [Tooltip("Time  it takes to reach each level of drift boost.")]
    [SerializeField] public float[] driftTimeThreshold = { 0,1,2 };
    [Tooltip("Accelerating force applied depending on time elapsed while drifting and driftTimeThresold.")]
    [SerializeField] public float[] driftBoosts = { 0,0,0 };
    [SerializeField] public Color[] boostColors;
    private int driftBoostIndex;

    [Tooltip("drift particles that appear on the right of player.")]
    [SerializeField] private ParticleSystem rightDriftParticles;
    private ParticleSystem.MainModule rDPMain;
    [Tooltip("drift particles that appear on the left of player.")]
    [SerializeField] private ParticleSystem leftDriftParticles;
    private ParticleSystem.MainModule lDPMain;
    [Tooltip("Speedboost UI effect.")]
    [SerializeField] private ImageAnimation speedBoostAnimation;
    private Image speedBoostImage;
    private SpeedBoost speedBoost;
    [Tooltip("Color of speed boost and drift particles while drifting, depending on time elapsed and driftTimeThreshold.")]
    /// <summary>
    /// Finds necessary components to reference and initializes state properties.
    /// </summary>
    private void Awake()
    {
        steer = GetComponent<Steer>();
        rb = transform.parent.GetComponentInChildren<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerMovement = GetComponent<PlayerMovement>();
        drifting = false;
        finishedDrifting = true;

        rDPMain = rightDriftParticles.main;
        lDPMain = leftDriftParticles.main;
        rightDriftParticles.gameObject.SetActive(false);
        leftDriftParticles.gameObject.SetActive(false);

        if(speedBoostAnimation != null) 
        {
            speedBoostImage = speedBoostAnimation.gameObject.GetComponent<Image>();
            speedBoost = speedBoostImage.GetComponent<SpeedBoost>();
        }
    }
    /// <summary>
    /// Handles drifting behavior while drifting input is received.
    /// </summary>
    /// <param name="inputDirection"> The direction input for steering. </param>
    public void Drifting(float inputDirection)
    {
        if(drifting)
        {
            if (Mathf.RoundToInt(inputDirection) == driftInitDirection) { SharpDrift(); }
            else if (Mathf.RoundToInt(inputDirection) == -1 * driftInitDirection) { WideDrift(); }
            else { NormalDrift(); }
            HandleDriftTime();
        }
    }
    /// <summary>
    /// Handles preparation of drifting behavior when drifting input is first received.
    /// </summary>
    /// <param name="inputDirection"> The direction input for steering. </param>
    public void StartDrift(float inputDirection) 
    {
        if(inputDirection == 0) { return; }
        if(inputDirection > 0) { leftDriftParticles.gameObject.SetActive(true); }
        else { rightDriftParticles.gameObject.SetActive(true); }
        driftInitDirection = Mathf.RoundToInt(inputDirection);
        playerAnimation.DisableMoveAnimations();
        playerAnimation.SetStartDrift();
        steer.enabled = false;
        SetUpDriftVariables();
        AudioManager.Instance.PlaySFX(5);
    }
    /// <summary>
    /// Handles the finishing touches of drifting behavior when drifting input is released.
    /// </summary>
    public void EndDrift() 
    {
        if(drifting) 
        { 
            playerAnimation.SetEndDrift();
            rightDriftParticles.gameObject.SetActive(false);
            leftDriftParticles.gameObject.SetActive(false);
            drifting = false;
            HandleFinishingDrift();
        }
    }
    /// <summary>
    /// Sets up state variables to their initial values when drifting first begins.
    /// </summary>
    private void SetUpDriftVariables()
    {
        drifting = true;
        finishedDrifting = false;
        startDriftTime = Time.time;
        driftBoostIndex = -1;
        steer.steerRate = sharpSteerRate; 
        playerMovement.maxSpeed = playerMovement.maxSpeed * driftSlowRate;
        playerMovement.accelerateForce = playerMovement.accelerateForce * driftSlowRate;
    }
    /// <summary>
    /// Manipulates the steering rate when drifting for sharp turns.
    /// </summary>
    private void SharpDrift() 
    {
        steer.steerRate = sharpSteerRate;
        steer.Steering(driftInitDirection); 

    }
    /// <summary>
    /// Manipulates the steering rate when drifting for wide turns.
    /// </summary>
    private void WideDrift() 
    {
        steer.steerRate = wideSteerRate;
        steer.Steering(driftInitDirection);
    }
    /// <summary>
    /// Manipulates the steering rate when drifting without sharp/wide turns.
    /// </summary>
    private void NormalDrift()
    {
        steer.steerRate = normalSteerRate;
        steer.Steering(driftInitDirection);
    }
    /// <summary>
    /// Reverts changes made to the movement, steering, and other components when SetUpDriftVariables() was called, back to default values.
    /// </summary>
    private void HandleFinishingDrift() 
    {
        AudioManager.Instance.PauseSFX(5);
        finishedDrifting = true;
        steer.steerRate = steer.baseSteerRate;
        playerMovement.maxSpeed = playerMovement.defaultMaxSpeed;
        playerMovement.accelerateForce = playerMovement.defaultAccelerateForce;
        steer.enabled = true;
        playerAnimation.EnableMoveAnimations();
        ActivateDriftBoost();
    }
    /// <summary>
    /// Handles changes to particle color and speed boost when drifting input is released, based on driftTimeThreshold.
    /// </summary>
    private void HandleDriftTime() 
    {
        for(int i=0; i < driftTimeThreshold.Length; i++)
        {
            if (i > driftBoostIndex && Time.time - startDriftTime > driftTimeThreshold[i])
            {
                driftBoostIndex = i;
            }            
            if(driftBoostIndex == -1)
            {
                rDPMain.startColor = Color.white;
                lDPMain.startColor = Color.white;
            }
            else
            {
                rDPMain.startColor = boostColors[driftBoostIndex];
                lDPMain.startColor = boostColors[driftBoostIndex];
            }
        }
    }
    /// <summary>
    /// Handles activation of speed boost.
    /// </summary>
    private void ActivateDriftBoost() 
    {
        if(driftBoostIndex == -1) { return; }
        ApplySpeedBoost(driftBoosts[driftBoostIndex], rb, steer.newDirection);
    }
    /// <summary>
    /// Handles adding speed boost as a force propelling the player, and sets up the speed boost animation to be played/stopped.
    /// </summary>
    /// <param name="boost"> The speed boost value. </param>
    /// <param name="rb"> The Rigidbody the speed boost is applied to. </param>
    /// <param name="direction"> The direction the speed boost is applied to. </param>
    void ApplySpeedBoost(float boost, Rigidbody rb, Vector3 direction)
    {
        rb.AddForce(boost * direction, ForceMode.Impulse);
        if(speedBoostAnimation == null) { return; }
        else 
        {
            speedBoostImage.color = boostColors[driftBoostIndex];
            speedBoostImage.color = new Vector4(speedBoostImage.color.r, speedBoostImage.color.g, speedBoostImage.color.b, .1f);
            speedBoost.SetTimeElapsed(0);
            speedBoost.SetDurationTime(driftBoostIndex / 2 + 1);
            speedBoost.gameObject.SetActive(true);
        }
    }
}
