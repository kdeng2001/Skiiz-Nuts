using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ensures the UI element follows the position of the followTarget.
/// </summary>
public class UIFollow : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    /// <summary>
    /// Follow the target at every frame.
    /// </summary>
    private void Update()
    {
        UIFollowObject();
    }

    /// <summary>
    /// Sets the image screen position to the screen position of the followTarget.
    /// </summary>
    private void UIFollowObject()
    {
        transform.position = RectTransformUtility.WorldToScreenPoint(
            Camera.main, 
            followTarget.TransformPoint(Vector3.zero)
        );
    }
}
