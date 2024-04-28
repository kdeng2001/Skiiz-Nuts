using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steer : MonoBehaviour
{
    private Rigidbody rb;    
    Grounding ground;
    public Vector3 direction { get; private set; }
    public Vector3 newDirection { get; private set; }
    public float yAngle { get; private set; }
    [Tooltip("The base rate at which the player can steer left or right. This variable should not be modified.")]
    [SerializeField] public float baseSteerRate = 2;
    public float steerRate;
    private void Awake()
    {
        rb = transform.parent.GetComponentInChildren<Rigidbody>();
        ground = GetComponent<Grounding>();
        yAngle = (int) transform.rotation.eulerAngles.y;
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (yAngle - 90)), 0, Mathf.Sin(Mathf.Deg2Rad * (yAngle + 90)));
        steerRate = baseSteerRate;
    }
    /// <summary>
    /// Steers player in a given direction to allow turning of corners
    /// </summary>
    /// <param name="inputDirection">-1,0,1 depending on horizontal input of player </param>
    public void Steering(float inputDirection)
    {
        if (yAngle > 360) { yAngle = yAngle - 360; }
        if (yAngle < 360) { yAngle = yAngle + 360; }
        yAngle += Mathf.RoundToInt(inputDirection) * steerRate;
        SetDirection(inputDirection);
        SphereDebug();
    }

    public void SetDirection(float inputDirection)
    {
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (yAngle - 90)), 0,
        Mathf.Sin(Mathf.Deg2Rad * (yAngle + 90)));

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, ground.groundCheckLayerMask) && ground.IsGrounded(transform.up * -1))
        {
            GroundedMovement(hit);
            // X and Z axis rotation depending on ground
            float z = GetAngleFromWorld(hit.normal, transform.up, newDirection);
            float x = GetAngleFromWorld(newDirection, transform.forward);
            Quaternion targetRotation = Quaternion.Euler(new (transform.eulerAngles.x + x, yAngle, transform.eulerAngles.z + z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime);
        }
        else { InAirMovement();  } 
        // horizontal rotation (AD keys)
        RotatePlayerSpriteY();
    }

    private float GetAngleFromWorld(Vector3 targetDirection, Vector3 currentDirection)
    {
        Vector3 axis = Vector3.Cross(currentDirection, targetDirection);
        float angle = Vector3.SignedAngle(currentDirection, targetDirection, axis);
        if (currentDirection.y > targetDirection.y) { return angle; }
        return -1 * angle;
    }

    private float GetAngleFromWorld(Vector3 targetDirection, Vector3 currentDirection, Vector3 axis)
    {
        float angle = Vector3.SignedAngle(currentDirection, targetDirection, axis);
        return  angle;
    }
    private void SphereDebug()
    {
        Debug.DrawLine(transform.position, transform.position + 10 * rb.transform.forward, Color.red);
        Debug.DrawLine(transform.position, transform.position + 10 * rb.transform.up, Color.green);
        Debug.DrawLine(transform.position, transform.position + 10 * rb.transform.right, Color.blue);
    }
    
    private void GroundedMovement(RaycastHit ground)
    {
        Vector3 projected = Vector3.Project(direction, ground.normal);
        newDirection = (direction - projected).normalized;
        //rb.velocity = rb.velocity.magnitude * newDirection;
        rb.AddForce(-rb.velocity + rb.velocity.magnitude * newDirection, ForceMode.VelocityChange);
    }

    private void InAirMovement()
    {
        //newDirection = direction;
        Vector3 yVelocity = Vector3.up * rb.velocity.y;
        Vector3 xzVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        newDirection = (xzVelocity + yVelocity).normalized;
        //rb.velocity = directionVelocity.magnitude * direction + yVelocity;
        rb.AddForce(-rb.velocity + xzVelocity.magnitude * direction + yVelocity, ForceMode.VelocityChange);
    }
    private void RotatePlayerSpriteY() { transform.eulerAngles = new(transform.eulerAngles.x, yAngle, transform.eulerAngles.z); }
}

