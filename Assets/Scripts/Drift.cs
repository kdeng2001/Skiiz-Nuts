using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drift : MonoBehaviour
{
    PlayerAnimation playerAnimation;
    Steer steer;
    Rigidbody rb;
    public bool drifting { get; private set; }
    public bool finishedDrifting { get; private set; }
    public float StartDriftTime { get; private set; }
    public int driftInitDirection { get; private set; }
    
    [SerializeField] public float sharpSteerRate = 4;
    [SerializeField] public float normalSteerRate = 2;
    [SerializeField] public float wideSteerRate = 1;
    [SerializeField] public float[] driftTimeThreshold = { 0,1,2 };
    [SerializeField] public float[] driftBoosts = { 0,0,0 };
    private int driftBoostIndex;

    [SerializeField] ParticleSystem rightDriftParticles;
    ParticleSystem.MainModule rDPMain;
    [SerializeField] ParticleSystem leftDriftParticles;
    ParticleSystem.MainModule lDPMain;
    [SerializeField] ParticleSystem speedBoostParticles;

    [SerializeField] ImageAnimation speedBoostAnimation;
    private Image speedBoostImage; 
    public Color[] boostColors;
    private void Awake()
    {
        steer = GetComponent<Steer>();
        rb = transform.parent.GetComponentInChildren<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        drifting = false;
        finishedDrifting = true;

        rDPMain = rightDriftParticles.main;
        lDPMain = leftDriftParticles.main;
        rightDriftParticles.gameObject.SetActive(false);
        leftDriftParticles.gameObject.SetActive(false);

        if(speedBoostAnimation != null) 
        {
            speedBoostImage = speedBoostAnimation.gameObject.GetComponent<Image>();
            speedBoostAnimation.gameObject.SetActive(false); 
        }
    }

    public void StartDrift(float inputDirection) 
    {
        if(inputDirection == 0) { Debug.Log("drift needs direction"); return; }
        if(inputDirection == -1) { leftDriftParticles.gameObject.SetActive(true); }
        else { rightDriftParticles.gameObject.SetActive(true); }
        driftInitDirection = Mathf.RoundToInt(inputDirection);
        playerAnimation.DisableMoveAnimations();
        playerAnimation.SetStartDrift();
        steer.enabled = false;
        SetUpDriftVariables();
        //Debug.Log("start drifting");
    }

    private void SetUpDriftVariables()
    {
        drifting = true;
        finishedDrifting = false;
        StartDriftTime = Time.time;
        driftBoostIndex = -1;
        steer.steerRate = sharpSteerRate; 
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
        finishedDrifting = true;
        steer.steerRate = steer.baseSteerRate;
        steer.enabled = true;
        playerAnimation.EnableMoveAnimations();
        ActivateDriftBoost();
        Debug.Log("finish drifting");
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
        if(speedBoostAnimation == null) { return; }
        else { StartCoroutine(HandleSpeedBoostAnimation()); }
        
    }

    IEnumerator HandleSpeedBoostAnimation()
    {
        speedBoostImage.color = boostColors[driftBoostIndex];
        speedBoostImage.color = new Vector4(speedBoostImage.color.r, speedBoostImage.color.g, speedBoostImage.color.b, .1f);

        speedBoostAnimation.gameObject.SetActive(true);
        yield return new WaitForSeconds(driftBoostIndex/2 + 1);
        speedBoostAnimation.gameObject.SetActive(false);
    }
}
