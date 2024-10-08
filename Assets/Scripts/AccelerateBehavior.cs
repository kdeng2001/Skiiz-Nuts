using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerateBehavior : StateMachineBehaviour
{
    private float currentLoopIndex = 0.5f;
    [SerializeField] private int[] sfxToPlay;
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        Debug.Log("normalized time: " + stateInfo.normalizedTime);
        if (stateInfo.normalizedTime > currentLoopIndex)
        {
            int sfx = (int)Mathf.Floor(Random.Range(0, 5));
            //int sfx = 5;
            AudioManager.Instance.PlaySFX(sfx);
            currentLoopIndex++;
            Debug.Log("onstateupdate");
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        currentLoopIndex = 0.5f;
    }
}
