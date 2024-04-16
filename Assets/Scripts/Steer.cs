using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steer : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 direction { get; private set; }
    public int currentAngle { get; private set; }
    [SerializeField] public int baseSteerRate = 2;
    [SerializeField] public bool limitRotation = false;
    [SerializeField] public int maxRotation = 60;

    public int steerRate;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentAngle = (int) transform.rotation.eulerAngles.y;
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (currentAngle - 90)), 0,
        Mathf.Sin(Mathf.Deg2Rad * (currentAngle + 90)));
        steerRate = baseSteerRate;
    }
    /// <summary>
    /// Steers player in a given direction to allow turning of corners
    /// </summary>
    /// <param name="inputDirection">-1,0,1 depending on horizontal input of player </param>
    public void Steering(float inputDirection)
    {
        if(inputDirection == 0 && transform.eulerAngles.y == currentAngle) { return; }
        if(limitRotation && currentAngle >= maxRotation) { return; }
        currentAngle += Mathf.RoundToInt(inputDirection) * steerRate;
        //transform.rotation = Quaternion.Euler(0, currentAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, currentAngle, 0), Time.deltaTime * 5);
        //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, currentAngle, 0), Time.deltaTime);
        SetDirection();
        
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
