using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    private void Update()
    {
        UIFollowObject();
    }

    private void UIFollowObject()
    {
        transform.position = RectTransformUtility.WorldToScreenPoint(
            Camera.main, 
            followTarget.TransformPoint(Vector3.zero)
        );
    }
}
