using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float timeElapsed;
    public float durationTime;

    private void Awake()
    {
        Debug.Log("Speedboost awake");
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
            Debug.Log("finish boost:" + " time elapsed=" + timeElapsed + " durationTime=" + durationTime);
            gameObject.SetActive(false);
        }
        else if(isActiveAndEnabled){ timeElapsed += Time.deltaTime; Debug.Log("isActiveAndEnabled"); }
        else { Debug.Log("bruh?"); }
    }

    public void SetTimeElapsed(float time) { timeElapsed = time; }
    public void SetDurationTime(float time) { durationTime = time; }
}
