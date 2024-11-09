using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles the direction the player moves, as well as sprite rotation based on this direction and the ground.
/// </summary>
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
    
    /// <summary>
    /// Finds necessary components and initializes the current angle and steerRate.
    /// </summary>
    private void Awake()
    {
        rb = transform.parent.GetComponentInChildren<Rigidbody>();
        ground = GetComponent<Grounding>();
        yAngle = (int) transform.rotation.eulerAngles.y;
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (yAngle - 90)), 0, Mathf.Sin(Mathf.Deg2Rad * (yAngle + 90)));
        steerRate = baseSteerRate;
    }

    /// <summary>
    /// Steers player in a given direction to allow turning of corners.
    /// </summary>
    /// <param name="inputDirection">-1,0,1 depending on horizontal input of player </param>
    public void Steering(float inputDirection)
    {
        if (yAngle > 360) { yAngle = yAngle - 360; }
        if (yAngle < 360) { yAngle = yAngle + 360; }
        yAngle += Mathf.RoundToInt(inputDirection) * steerRate;
        SetDirection();
        //SphereDebug();
    }

    /// <summary>
    /// Determines direction of movement and rotation of sprite relative to the ground normal
    /// </summary>
    private void SetDirection()
    {
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (yAngle - 90)), 0,
        Mathf.Sin(Mathf.Deg2Rad * (yAngle + 90)));

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, ground.groundCheckLayerMask) && ground.IsGrounded(transform.up * -1))
        {
            GroundedMovement(hit);
            // X and Z axis rotation of sprite depending on ground
            float z = GetAngleFromWorld(hit.normal, transform.up, newDirection);
            float x = GetAngleFromWorld(newDirection, transform.forward);
            Quaternion targetRotation = Quaternion.Euler(new (transform.eulerAngles.x + x, yAngle, transform.eulerAngles.z + z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime);



            Debug.DrawLine(transform.position, transform.position + 10 * transform.forward, Color.red);
            Debug.DrawLine(transform.position, transform.position + 15 * newDirection, Color.blue);
        }
        else { InAirMovement();  } 
        // horizontal rotation (AD keys)
        RotatePlayerSpriteY();
    }

    /// <summary>
    /// Computes the angle between two vectors.
    /// </summary>
    /// <param name="targetDirection"> The direction vector the player will face. </param>
    /// <param name="currentDirection"> The direction vector the player currently faces. </param>
    /// <returns> Returns the angle between targetDirection and currentDirection. </returns>
    private float GetAngleFromWorld(Vector3 targetDirection, Vector3 currentDirection)
    {
        Vector3 axis = Vector3.Cross(currentDirection, targetDirection);
        float angle = Vector3.SignedAngle(currentDirection, targetDirection, axis);
        if (currentDirection.y > targetDirection.y) { return angle; }
        return -1 * angle;
    }

    /// <summary>
    /// Computes the angle between two vectors, in relation to a third axis vector.
    /// </summary>
    /// <param name="targetDirection"> The direction vector the player will face. </param>
    /// <param name="currentDirection"> The direction vector the player currently faces. </param>
    /// <param name="axis"></param>
    /// <returns> Returns the angle between targetDirection and currentDirection about a third axis vector. </returns>
    private float GetAngleFromWorld(Vector3 targetDirection, Vector3 currentDirection, Vector3 axis)
    {
        float angle = Vector3.SignedAngle(currentDirection, targetDirection, axis);
        return  angle;
    }
    
    /// <summary>
    /// Draws lines for debugging important vectors that will be manipulated with vector math.
    /// </summary>
    private void SphereDebug()
    {
        Debug.DrawLine(transform.position, transform.position + 10 * rb.transform.forward, Color.red);
        Debug.DrawLine(transform.position, transform.position + 10 * rb.transform.up, Color.green);
        Debug.DrawLine(transform.position, transform.position + 10 * rb.transform.right, Color.blue);
    }
    
    /// <summary>
    /// For additional vector lines that can be drawn to help debug and verify correct use of vector math.
    /// </summary>
    private void OtherDebugs()
    {
        Debug.DrawLine(transform.position, transform.position + 10 * transform.forward, Color.red);
    }

    /// <summary>
    /// Handles direction to apply velocity when the player is on the ground.
    /// </summary>
    /// <param name="ground"></param>
    private void GroundedMovement(RaycastHit ground)
    {
        Vector3 projected = Vector3.Project(direction, ground.normal);
        newDirection = (direction - projected).normalized;
        rb.AddForce(-rb.velocity + rb.velocity.magnitude * newDirection, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Handles direction to apply velocity when the player is above ground.
    /// </summary>
    private void InAirMovement()
    {
        Vector3 yVelocity = Vector3.up * rb.velocity.y;
        Vector3 xzVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        newDirection = (xzVelocity + yVelocity).normalized;
        rb.AddForce(-rb.velocity + xzVelocity.magnitude * direction + yVelocity, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Rotates the player sprite along the y axis only.
    /// </summary>
    private void RotatePlayerSpriteY() 
    { 
        transform.eulerAngles = new(transform.eulerAngles.x, yAngle, transform.eulerAngles.z);
    }
}

