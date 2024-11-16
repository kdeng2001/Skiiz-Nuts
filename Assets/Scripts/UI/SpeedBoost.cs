using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float timeElapsed;
    public float durationTime;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        timeElapsed = 0f;
    }

    private void Update()
    {
        if(timeElapsed > durationTime)
        {
            gameObject.SetActive(false);
        }
        else if(isActiveAndEnabled){ timeElapsed += Time.deltaTime;}
    }

    public void SetTimeElapsed(float time) { timeElapsed = time; }
    public void SetDurationTime(float time) { durationTime = time; }
}
