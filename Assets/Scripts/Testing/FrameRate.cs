using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRate : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public int frameRate = 5;
    void Start()
    {
        Application.targetFrameRate = frameRate;
    }

    private void Update()
    {
        if(Application.targetFrameRate != frameRate)
        {
            Application.targetFrameRate = frameRate;
        }
    }
}
