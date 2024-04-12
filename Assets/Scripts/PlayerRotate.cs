using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 direction { get; private set; }
    public int currentAngle { get; private set; }
    [SerializeField] public int rotateRate = 2;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentAngle = 0;
        direction = Vector3.forward;
    }
    public void Rotate()
    {
        //Quaternion.FromToRotation(forwardQuaternion)
        if (PlayerActionManager.Instance.moveValue.x > 0)
        {
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, -60), rotateRate);
            if (currentAngle < 60) { currentAngle += rotateRate; }
            SetDirection();
        }
        else if (PlayerActionManager.Instance.moveValue.x < 0)
        {
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 60), rotateRate);
            if (currentAngle > -60) { currentAngle -= rotateRate; }
            SetDirection();
        }
        Debug.DrawLine(transform.position, transform.position + 10 * direction, Color.red);
    }

    public void SetDirection()
    {
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (currentAngle - 90)), 0,
        Mathf.Sin(Mathf.Deg2Rad * (currentAngle + 90)));
        //Debug.Log(direction);
        rb.velocity = rb.velocity.magnitude * direction;
    }
}
