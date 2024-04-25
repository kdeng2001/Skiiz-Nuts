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
        //SphereDebug();
        //Debug.DrawLine(transform.position, transform.position + 5 * direction, Color.cyan);
    }

    public void SetDirection(float inputDirection)
    {
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (yAngle - 90)), 0,
        Mathf.Sin(Mathf.Deg2Rad * (yAngle + 90)));

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, ground.groundCheckLayerMask) && ground.IsGrounded(transform.up * -1))
        {
            GroundedMovement(hit);
            //RotatePlayerSpriteZ(hit.normal, direction);
            //float z = GetAngleFromCamera(hit.normal, newDirection, transform.position);
            float z = GetAngleFromWorld(hit.normal, transform.up, newDirection);
            float x = GetAngleFromWorld(newDirection, transform.forward);
            //float x = 0;
            //Debug.Log("x: " + x + " euler x: " + transform.eulerAngles.x + "z: " + x + " euler z: " + transform.eulerAngles.z);
            //if (Mathf.Abs(z) > 5) { z *= Time.fixedDeltaTime; }
            //if (Mathf.Abs(x) > 5) { x *= Time.fixedDeltaTime; }
            //transform.eulerAngles = new(
            //    transform.eulerAngles.x + x,
            //    yAngle,
            //    (transform.eulerAngles.z) + z
            //    );
            transform.Rotate(x, yAngle - transform.eulerAngles.y, z);
        }
        else { InAirMovement();  } // horizontal rotation (AD keys)
        RotatePlayerSpriteY();
        rb.AddForce(Vector3.down * ground.gravity * 5f, ForceMode.Acceleration); // gravity
    }

    private float GetAngleFromWorld(Vector3 targetDirection, Vector3 currentDirection)
    {
        //Debug.Log("transform forward: " + currentDirection + ", targetDirection: " + (targetDirection - transform.position));
        Vector3 axis = Vector3.Cross(currentDirection, targetDirection);
        float angle = Vector3.SignedAngle(currentDirection, targetDirection, axis);
        //Debug.DrawLine(transform.position, transform.position + 10 * currentDirection, Color.red);
        //Debug.DrawLine(transform.position, transform.position + 10 * (targetDirection), Color.green);
        //Debug.DrawLine(transform.position, transform.position + 20 * (axis), Color.yellow);
        if (currentDirection.y > targetDirection.y) { return angle; }
        return -1 * angle;
        //return angle;
    }

    private float GetAngleFromWorld(Vector3 targetDirection, Vector3 currentDirection, Vector3 vertex)
    {
        
        float angle = Vector3.SignedAngle(currentDirection, targetDirection, vertex);
        Debug.Log("transform up: " + currentDirection + ", targetDirection: " + (targetDirection) + ", angle: " + angle);
        Debug.DrawLine(transform.position, transform.position + 10 * currentDirection, Color.red);
        Debug.DrawLine(transform.position, transform.position + 10 * (targetDirection), Color.green);
        Debug.DrawLine(transform.position, transform.position + 20 * (vertex), Color.yellow);
        //if (currentDirection.x > targetDirection.x) { return-1 *  angle; }
        return  angle;
        //return angle;
    }

    //private float GetAngleFromCamera(Vector3 targetDirection, Vector3 currentDirection, Vector3 vertex)
    //{
    //    Vector2 normalPoint = Camera.main.WorldToScreenPoint(vertex + targetDirection);
    //    Vector2 facePoint = Camera.main.WorldToScreenPoint(vertex + currentDirection);
    //    Vector2 playerPoint = Camera.main.WorldToScreenPoint(vertex);
    //    Debug.DrawLine(transform.position, vertex + 10 * currentDirection, Color.red);
    //    Debug.DrawLine(transform.position, vertex + 10 * (targetDirection), Color.green);
    //    float angle = GetAngle2D(facePoint, normalPoint, playerPoint);
    //    if (facePoint.x > normalPoint.x) { return angle; }
    //    else { return -1 * angle; }
    //    //return angle;
    //}

    //private void RotatePlayerSpriteZ(Vector3 targetDirection, Vector3 currentDirection)
    //{
    //    Vector2 normalPoint = Camera.main.WorldToScreenPoint(transform.position + targetDirection);        
    //    Vector2 facePoint = Camera.main.WorldToScreenPoint(transform.position + currentDirection);
    //    Vector2 playerPoint = Camera.main.WorldToScreenPoint(transform.position);
    //    float angle = GetAngle2D(facePoint, normalPoint, playerPoint);
    //    if (facePoint.x > normalPoint.x)
    //    {
    //        transform.eulerAngles += Vector3.forward * angle * Time.fixedDeltaTime;
    //        //rb.rotation = Quaternion.Euler(0, rb.rotation.y, angle * Time.fixedDeltaTime * .1f);
    //    }
    //    else
    //    {
    //        transform.eulerAngles += Vector3.back * angle * Time.fixedDeltaTime;
    //        //rb.rotation = Quaternion.Euler(0, rb.rotation.y, -1 * angle * Time.fixedDeltaTime * .1f);
    //    }
    //}    
    private float GetAngle2D(Vector2 p1, Vector2 p2, Vector2 vertex) 
    {
        return (Vector2.Angle(p1 - vertex, p2 - vertex));
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
        rb.velocity = rb.velocity.magnitude * newDirection;

        //rb.velocity = rb.velocity.magnitude * direction;
        //transform.eulerAngles = new(newDirection.x, transform.eulerAngles.y, transform.eulerAngles.z);
        //transform.up = ground.normal;
        //Debug.DrawLine(transform.position, transform.position + 10 * newDirection, Color.yellow);
        //Debug.DrawLine(transform.position, transform.position + 10 * ground.normal, Color.blue);
    }

    private void InAirMovement()
    {
        newDirection = direction;
        Vector3 yVelocity = Vector3.up * rb.velocity.y;
        Vector3 directionVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.velocity = directionVelocity.magnitude * direction + yVelocity;
    }
    private void RotatePlayerSpriteY() { transform.eulerAngles = new(transform.eulerAngles.x, yAngle, transform.eulerAngles.z); }
}

