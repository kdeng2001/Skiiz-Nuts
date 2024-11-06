using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public delegate void PauseMoveAnimation();
    public PauseMoveAnimation onPauseMoveAnimation;

    public delegate void StartDrift(float input);
    public StartDrift onStartDrift;
    public delegate void EndDrift();
    public EndDrift onEndDrift;

    public delegate void SpeedBoost(float boost, Rigidbody rb, Vector3 direction);
}
