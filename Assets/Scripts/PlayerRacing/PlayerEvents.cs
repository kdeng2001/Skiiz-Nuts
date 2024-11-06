using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public delegate void StartDrift(float input);
    public StartDrift onStartDrift;
    public delegate void EndDrift();
    public EndDrift onEndDrift;
}
