using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steer : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 direction { get; private set; }
    public Vector3 newDirection { get; private set; }
    public float currentAngle { get; private set; }
    [SerializeField] public float baseSteerRate = 2;
    [SerializeField] public bool limitRotation = false;
    [SerializeField] public float maxRotation = 60;

    public float steerRate;
    Grounding ground;
    private void Awake()
    {
        rb = transform.parent.GetComponentInChildren<Rigidbody>();
        currentAngle = (int) transform.rotation.eulerAngles.y;
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (currentAngle - 90)), 0,
        Mathf.Sin(Mathf.Deg2Rad * (currentAngle + 90)));
        steerRate = baseSteerRate;
        ground = GetComponent<Grounding>();
    }
    /// <summary>
    /// Steers player in a given direction to allow turning of corners
    /// </summary>
    /// <param name="inputDirection">-1,0,1 depending on horizontal input of player </param>
    public void Steering(float inputDirection)
    {
        if(inputDirection == 0 && transform.eulerAngles.y == currentAngle) { return; }
        if(limitRotation && currentAngle >= maxRotation) { return; }
        currentAngle += inputDirection * steerRate;
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


        

        if(ground.IsGrounded()) 
        {        
            rb.velocity = rb.velocity.magnitude * newDirection.normalized;
            RaycastHit hit;
            //Debug.Log(direction);
            Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, ground.groundCheckLayerMask);
            if (Mathf.Abs(direction.z) > 0.5f)
            {
                if (direction.z > 0 && direction.x >= 0) { newDirection = new Vector3(direction.x, (Quaternion.Euler(90, 0, 0) * hit.normal).y, direction.z); }
                else if (direction.z > 0 && direction.x <= 0) { newDirection = new Vector3(direction.x, (Quaternion.Euler(90, 0, 0) * hit.normal).y, direction.z); }
                else if (direction.z < 0 && direction.x >= 0) { newDirection = new Vector3(direction.x, -1 * (Quaternion.Euler(90, 0, 0) * hit.normal).y, direction.z); }
                else if (direction.z < 0 && direction.x <= 0) { newDirection = new Vector3(direction.x, -1 * (Quaternion.Euler(90, 0, 0) * hit.normal).y, direction.z); }
            }
            else
            {
                if (direction.z > 0 && direction.x >= 0) { newDirection = new Vector3(direction.x, -1 * (Quaternion.Euler(0, 0, 90) * hit.normal).y, direction.z); }
                else if (direction.z > 0 && direction.x <= 0) { newDirection = new Vector3(direction.x, (Quaternion.Euler(0, 0, 90) * hit.normal).y, direction.z); }
                else if (direction.z < 0 && direction.x >= 0) { newDirection = new Vector3(direction.x, -1 * (Quaternion.Euler(0, 0, 90) * hit.normal).y, direction.z); }
                else if (direction.z < 0 && direction.x <= 0) { newDirection = new Vector3(direction.x, (Quaternion.Euler(0, 0, 90) * hit.normal).y, direction.z); }
            }
            Debug.DrawLine(transform.position, transform.position + newDirection * 10, Color.yellow);
        }
        else
        {
            newDirection = direction;
            Vector3 yVelocity = Vector3.up * rb.velocity.y;
            Vector3 directionVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.velocity = directionVelocity.magnitude * direction + yVelocity;
        }

    }
}
