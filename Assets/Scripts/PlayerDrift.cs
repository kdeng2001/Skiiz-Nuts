using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrift : MonoBehaviour
{
    PlayerController playerController;
    PlayerAnimation playerAnimation;
    PlayerRotate playerRotate;
    public bool drifting { get; private set; }
    public bool finishedDrifting { get; private set; }
    public float StartDriftTime { get; private set; }

    [SerializeField] public int driftSharpRotateRate = 6;
    [SerializeField] public int driftWideRotateRate = 4;
    [SerializeField] public float[] driftTimeThreshold = { 0,1,2 };
    [SerializeField] public float[] driftBoosts = { 0,0,0 };
    private int driftBoostIndex;
    
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerRotate = GetComponent<PlayerRotate>();
        playerAnimation = GetComponent<PlayerAnimation>();
        drifting = false;
        finishedDrifting = true;
    }
    private void OnEnable()
    {
        PlayerEvents.Instance.onStartDrift += StartDrift;
        PlayerEvents.Instance.onEndDrift += EndDrift;
    }

    private void OnDisable()
    {
        PlayerEvents.Instance.onStartDrift -= StartDrift;
        PlayerEvents.Instance.onEndDrift -= EndDrift;
    }



    public void StartDrift() 
    {
        if(PlayerActionManager.Instance.moveValue.x != 0 && !drifting)
        {
            playerAnimation.DisableMoveAnimations();
            playerAnimation.SetStartDrift();
            Debug.Log("start drifting");
            SetUpDriftVariables();
        }
        else { Debug.Log("drifting"); return; }
    }

    private void SetUpDriftVariables()
    {
        drifting = true;
        finishedDrifting = false;
        StartDriftTime = Time.time;
        driftBoostIndex = 0;
        playerRotate.rotateRate = driftSharpRotateRate;
    }

    public void EndDrift() 
    {
        if(drifting) 
        { 
            playerAnimation.SetEndDrift();
            drifting = false;
        }
        
    }    
    public void Drift()
    {
        HandleFinishingDrift();
        if(drifting)
        {
            HandleDriftTime();
        }
    }

    private void HandleFinishingDrift() 
    {         
        if(!finishedDrifting && playerAnimation.AnimationHasEnded(playerAnimation.EndDrifting)) 
        {
            finishedDrifting = true;
            playerRotate.rotateRate = playerRotate.baseRotateRate;
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

    private void ActivateDriftBoost() { Debug.Log("Drift BOOOST " + driftBoostIndex + "!!!"); }
}
