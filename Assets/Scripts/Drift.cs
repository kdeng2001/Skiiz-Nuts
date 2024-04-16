using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour
{
    PlayerAnimation playerAnimation;
    Steer steer;
    Rigidbody rb;
    public bool drifting { get; private set; }
    public bool finishedDrifting { get; private set; }
    public float StartDriftTime { get; private set; }
    public int driftInitDirection { get; private set; }
    
    [SerializeField] public int sharpSteerRate = 4;
    [SerializeField] public int normalSteerRate = 2;
    [SerializeField] public int wideSteerRate = 1;
    [SerializeField] public float[] driftTimeThreshold = { 0,1,2 };
    [SerializeField] public float[] driftBoosts = { 0,0,0 };
    private int driftBoostIndex;

    
    private void Awake()
    {
        steer = GetComponent<Steer>();
        rb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        drifting = false;
        finishedDrifting = true;
    }

    public void StartDrift(float inputDirection) 
    {
        if(inputDirection == 0) { Debug.Log("drift needs direction"); return; }
        driftInitDirection = Mathf.RoundToInt(inputDirection);
        playerAnimation.DisableMoveAnimations();
        playerAnimation.SetStartDrift();
        steer.enabled = false;
        SetUpDriftVariables();
        Debug.Log("start drifting");
    }

    private void SetUpDriftVariables()
    {
        drifting = true;
        finishedDrifting = false;
        StartDriftTime = Time.time;
        driftBoostIndex = -1;
        steer.steerRate = sharpSteerRate;
    }

    public void EndDrift() 
    {
        if(drifting) 
        { 
            playerAnimation.SetEndDrift();
            drifting = false;
        }
        
    }    
    public void Drifting(float inputDirection)
    {
        HandleFinishingDrift();
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

    private void HandleFinishingDrift() 
    {         
        if(!finishedDrifting && playerAnimation.AnimationHasEnded(playerAnimation.EndDrifting)) 
        {
            //drifting = false;
            finishedDrifting = true;
            steer.steerRate = steer.baseSteerRate;
            steer.enabled = true;
            playerAnimation.EnableMoveAnimations();
            ActivateDriftBoost();
        }
    }

    private void HandleDriftTime() 
    {
        for(int i=0; i < driftTimeThreshold.Length; i++)
        {
            if(i > driftBoostIndex && Time.time - StartDriftTime > driftTimeThreshold[i])
            {
                Debug.Log("reach drift boost " + i);
                driftBoostIndex = i;
            }
        }
    }

    private void ActivateDriftBoost() 
    {
        Debug.Log("Drift BOOOST " + driftBoostIndex + "!!!");
        if(driftBoostIndex == -1) { return; }
        ApplySpeedBoost(driftBoosts[driftBoostIndex], rb, steer.direction);
    }

    void ApplySpeedBoost(float boost, Rigidbody rb, Vector3 direction)
    {
        rb.AddForce(boost * direction, ForceMode.Impulse);
    }
}
