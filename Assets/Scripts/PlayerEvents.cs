using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public static PlayerEvents Instance;
    private void Awake()
    {
        if(Instance != null && Instance != this) { Destroy(gameObject); return; }
        PlayerEvents.Instance = this;
    }

    public delegate void PauseMoveAnimation();
    public PauseMoveAnimation onPauseMoveAnimation;

    public delegate void StartDrift();
    public StartDrift onStartDrift;
    public delegate void EndDrift();
    public EndDrift onEndDrift;
}
