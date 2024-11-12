using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// AccelerateBehavior inherits from StateMachineBehaviour and allows the skiing sfx
/// to sync with the accelerate animation for consistent sound.
/// </summary>
public class AccelerateBehavior : StateMachineBehaviour
{
    private float currentLoopIndex = 0.5f;
    [SerializeField] private int[] sfxToPlay;
    /// <summary>
    /// Plays an accelerate sfx every time the animation has reached the halfway mark (which loops).
    /// </summary>
    /// <param name="animator"> The animator evaluating this state machine. </param>
    /// <param name="stateInfo"> The info about the state being evaluated. </param>
    /// <param name="layerIndex"> The current layer being evaluated.  </param>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        //Debug.Log($"normalized time: {stateInfo.normalizedTime}, currentLoopIndex: {currentLoopIndex}");
        if (stateInfo.normalizedTime > currentLoopIndex)
        {
            // picks from a random group of accelerate sound effects to play
            int sfx = (int)Mathf.Floor(Random.Range(0, 5));
            AudioManager.Instance.PlaySFX(sfx);
            currentLoopIndex++;
            Debug.Log("onstateupdate");
        }
    }
    /// <summary>
    /// Resets currentLoopIndex so it doesn't overflow.
    /// </summary>
    /// <param name="animator"> The animator evaluating this state machine. </param>
    /// <param name="stateInfo"> The info about the state being evaluated. </param>
    /// <param name="layerIndex"> The current layer being evaluated.  </param>
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("exit state");
        base.OnStateExit(animator, stateInfo, layerIndex);
        currentLoopIndex = 0.5f;
    }
}
