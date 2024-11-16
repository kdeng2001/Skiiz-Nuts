using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SpeedBoost contains functionality for the duration and end of the speed boost animation (since it loops).
/// </summary>
public class SpeedBoost : MonoBehaviour
{
    public float timeElapsed;
    public float durationTime;

    /// <summary>
    /// Hides the gameObject containing the speed boost animation.
    /// </summary>
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Resets the timeElapsed value to 0 when the gameObject is enabled.
    /// </summary>
    private void OnEnable()
    {
        timeElapsed = 0f;
    }

    /// <summary>
    /// Keeps track of time passed while animation is playing.
    /// If timeElapsed is greater than the specified duration of the animation loop, the animation is finished.
    /// </summary>
    private void Update()
    {
        if(timeElapsed > durationTime)
        {
            gameObject.SetActive(false);
        }
        else if(isActiveAndEnabled){ timeElapsed += Time.deltaTime;}
    }

    /// <summary>
    /// Manually sets the timeElapsed value to time parameter.
    /// </summary>
    /// <param name="time"> The time. </param>
    public void SetTimeElapsed(float time) { timeElapsed = time; }
    /// <summary>
    /// Manually sets the durationTime to time parameter.
    /// </summary>
    /// <param name="time"> The time. </param>
    public void SetDurationTime(float time) { durationTime = time; }
}
