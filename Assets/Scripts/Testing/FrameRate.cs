using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FrameRate is a component that when attached to any gameObject, will make the frame rate equal to the frameRate property.
/// It's useful for debugging issues that arise different frame rate between users.
/// </summary>
public class FrameRate : MonoBehaviour
{
    [SerializeField] public int frameRate = 5;
    /// <summary>
    /// Sets the frame rate to what frameRate property is initially.
    /// </summary>
    private void Start()
    {
        Application.targetFrameRate = frameRate;
    }
    /// <summary>
    /// Changes the frame rate if there are changes to frameRate value.
    /// This is useful for changing frameRate during playmode.
    /// </summary>
    private void Update()
    {
        if(Application.targetFrameRate != frameRate)
        {
            Application.targetFrameRate = frameRate;
        }
    }
}
