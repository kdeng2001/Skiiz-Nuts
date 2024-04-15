using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 direction { get; private set; }
    public int currentAngle { get; private set; }
    [SerializeField] public int baseRotateRate = 2;
    [SerializeField] public bool limitRotation = false;
    [SerializeField] public int maxRotation = 60;

    public int rotateRate;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentAngle = (int) transform.rotation.eulerAngles.y;
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (currentAngle - 90)), 0,
        Mathf.Sin(Mathf.Deg2Rad * (currentAngle + 90)));
        rotateRate = baseRotateRate;
    }
    public void Rotate()
    {
        //Quaternion.FromToRotation(forwardQuaternion)
        if (PlayerActionManager.Instance.moveValue.x > 0)
        {
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, maxRotation, 0), rotateRate);

            /*if (currentAngle < maxRotation)*/ 
            if(limitRotation && currentAngle >= maxRotation) { return; }
            { currentAngle += rotateRate; }
            transform.rotation = Quaternion.Euler(0, currentAngle, 0);
            SetDirection();
        }
        else if (PlayerActionManager.Instance.moveValue.x < 0)
        {

            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, -1 * maxRotation, 0), rotateRate);
           /* if (currentAngle > -1 * maxRotation)*/ 
            if(limitRotation && currentAngle <= -1*maxRotation) { return; }
            { currentAngle -= rotateRate; }
            transform.rotation = Quaternion.Euler(0, currentAngle, 0);
            SetDirection();
        }
        Debug.DrawLine(transform.position, transform.position + 10 * direction, Color.red);
    }

    public void SetDirection()
    {
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (currentAngle - 90)), 0,
        Mathf.Sin(Mathf.Deg2Rad * (currentAngle + 90)));
        //Debug.Log(direction);
        Vector3 yVelocity = Vector3.up * rb.velocity.y;
        Vector3 directionVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.velocity = directionVelocity.magnitude * direction + yVelocity;
    }
}
