using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Declares delegates invoked on certain player racing driven inputs,
/// like starting or ending drifting.
/// </summary>
public class PlayerEvents : MonoBehaviour
{
    public delegate void StartDrift(float input);
    public StartDrift onStartDrift;
    public delegate void EndDrift();
    public EndDrift onEndDrift;
}
