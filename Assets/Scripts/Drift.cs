using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drift : MonoBehaviour
{
    PlayerAnimation playerAnimation;
    Steer steer;
    Rigidbody rb;
    PlayerMovement playerMovement;
    public bool drifting { get; private set; }
    public bool finishedDrifting { get; private set; }
    public float StartDriftTime { get; private set; }
    public int driftInitDirection { get; private set; }
    [Tooltip("Multiplier that decreases acceleration and maxBaseSpeed (from ordinary movement) while drifting.")]
    [SerializeField] float driftSlowRate = .75f;
    
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
    private int driftBoostIndex;

    [Tooltip("drift particles that appear on the right of player.")]
    [SerializeField] ParticleSystem rightDriftParticles;
    ParticleSystem.MainModule rDPMain;
    [Tooltip("drift particles that appear on the left of player.")]
    [SerializeField] ParticleSystem leftDriftParticles;
    ParticleSystem.MainModule lDPMain;
    [Tooltip("Speedboost UI effect.")]
    [SerializeField] ImageAnimation speedBoostAnimation;
    private Image speedBoostImage;
    private SpeedBoost speedBoost;
    [Tooltip("Color of speed boost and drift particles while drifting, depending on time elapsed and driftTimeThreshold.")]
    [SerializeField] public Color[] boostColors;


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
            //speedBoostAnimation.gameObject.SetActive(false); 
        }
    }

    public void StartDrift(float inputDirection) 
    {
        if(inputDirection == 0) {/* Debug.Log("drift needs direction"); */return; }
        if(inputDirection > 0) { leftDriftParticles.gameObject.SetActive(true); }
        else { rightDriftParticles.gameObject.SetActive(true); }
        driftInitDirection = Mathf.RoundToInt(inputDirection);
        playerAnimation.DisableMoveAnimations();
        playerAnimation.SetStartDrift();
        steer.enabled = false;
        SetUpDriftVariables();
        AudioManager.Instance.PlaySFX(5);
        //Debug.Log("start drifting");
    }

    private void SetUpDriftVariables()
    {
        drifting = true;
        finishedDrifting = false;
        StartDriftTime = Time.time;
        driftBoostIndex = -1;
        steer.steerRate = sharpSteerRate; 
        playerMovement.maxSpeed = playerMovement.maxSpeed * driftSlowRate;
        playerMovement.accelerateForce = playerMovement.accelerateForce * driftSlowRate;
    }


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
    private void SharpDrift() 
    {
        steer.steerRate = sharpSteerRate;
        steer.Steering(driftInitDirection); 

    }
    private void WideDrift() 
    {
        steer.steerRate = wideSteerRate;
        steer.Steering(driftInitDirection);
    }

    public void NormalDrift()
    {
        steer.steerRate = normalSteerRate;
        steer.Steering(driftInitDirection);
    }
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
        //Debug.Log("finish drifting");
    }

    private void HandleDriftTime() 
    {
        for(int i=0; i < driftTimeThreshold.Length; i++)
        {
            if (i > driftBoostIndex && Time.time - StartDriftTime > driftTimeThreshold[i])
            {
                //Debug.Log("reach drift boost " + i);
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

    private void ActivateDriftBoost() 
    {
        //Debug.Log("Drift BOOOST " + driftBoostIndex + "!!!");
        if(driftBoostIndex == -1) { return; }
        ApplySpeedBoost(driftBoosts[driftBoostIndex], rb, steer.newDirection);
    }

    void ApplySpeedBoost(float boost, Rigidbody rb, Vector3 direction)
    {
        rb.AddForce(boost * direction, ForceMode.Impulse);
        if(speedBoostAnimation == null) { /*Debug.Log("null speedBoostAnimation");*/ return; }
        else 
        {
            //Debug.Log("Speedboost prep");
            speedBoostImage.color = boostColors[driftBoostIndex];
            speedBoostImage.color = new Vector4(speedBoostImage.color.r, speedBoostImage.color.g, speedBoostImage.color.b, .1f);
            speedBoost.SetTimeElapsed(0);
            speedBoost.SetDurationTime(driftBoostIndex / 2 + 1);
            speedBoost.gameObject.SetActive(true);
        }
    }
}
