using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steer : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 direction { get; private set; }
    public Vector3 newDirection { get; private set; }
    public float XZAngle { get; private set; }
    [SerializeField] public float baseSteerRate = 2;

    public float steerRate;
    Grounding ground;
    PlayerMovement playerMovement;
    private void Awake()
    {
        rb = transform.parent.GetComponentInChildren<Rigidbody>();
        XZAngle = (int) transform.rotation.eulerAngles.y;
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (XZAngle - 90)), 0,
        Mathf.Sin(Mathf.Deg2Rad * (XZAngle + 90)));
        steerRate = baseSteerRate;
        ground = GetComponent<Grounding>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    /// <summary>
    /// Steers player in a given direction to allow turning of corners
    /// </summary>
    /// <param name="inputDirection">-1,0,1 depending on horizontal input of player </param>
    public void Steering(float inputDirection)
    {
        XZAngle += Mathf.RoundToInt(inputDirection) * steerRate;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, XZAngle, 0), Time.deltaTime * 5);
        //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, XZAngle, 0), Time.deltaTime);
        SetDirection(inputDirection);
        
        Debug.DrawLine(transform.position, transform.position + 10 * direction, Color.red);
    }

    public void SetDirection(float inputDirection)
    {
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (XZAngle - 90)), 0,
        Mathf.Sin(Mathf.Deg2Rad * (XZAngle + 90)));

        RaycastHit hit;
        if (Physics.Raycast(transform.position + direction, Vector3.down, out hit, Mathf.Infinity, ground.groundCheckLayerMask) && ground.IsGrounded()) 
        {
            // Note: not sure why, but normalizing newDirection instead of hit.normal resolved issue of slowing down going up/down slopes
            // however, there is still sometimes an immediate drop in velocity when moving
            Vector3 projected = Vector3.Project(direction, hit.normal);
            newDirection = (direction - projected).normalized;

            rb.velocity = rb.velocity.magnitude * newDirection;
            //rb.AddTorque(newDirection);
            //rb.AddForce(inputDirection * playerMovement.accelerateForce * transform.right, ForceMode.Acceleration);
            //gravity
            rb.AddForce(-1 * hit.normal, ForceMode.Acceleration);

            Vector2 v1 = Camera.main.WorldToScreenPoint(transform.position + hit.normal);
            Vector2 playerPoint = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 facePoint = Camera.main.WorldToScreenPoint(transform.position + direction);
            //Vector2 v2 = Camera.main.WorldToViewportPoint(direction * 20);
            float angle = (Vector2.Angle(v1 - playerPoint, facePoint - playerPoint));
            //Debug.Log("v1: " + new Vector2Int((int)v1.x, (int)v1.y) +
            //    ", playerPoint: " + new Vector2Int((int)playerPoint.x, (int)playerPoint.y) + 
            //    ", facePoint: " + new Vector2Int((int)facePoint.x, (int)facePoint.y) +
            //    ", hit.normal: " + hit.normal +
            //    ", angle: " + angle);
            if (angle > 1)
            {
                // z rotation for sprite uneven surfaces
                if (facePoint.x > v1.x)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + angle), Time.deltaTime * .4f);
                }
                else
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - angle), Time.deltaTime * .4f);
                }
            }
        }
        else
        {
            // something here causes stuttering (when turning?)
            newDirection = direction;
            Vector3 yVelocity = Vector3.up * rb.velocity.y;
            Vector3 directionVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.velocity = directionVelocity.magnitude * direction + yVelocity;
            //rb.AddTorque(direction + yVelocity);
            //rb.AddForce(inputDirection * playerMovement.accelerateForce * transform.right, ForceMode.Acceleration);
            rb.AddForce(Vector3.down * ground.gravity * 5f, ForceMode.Acceleration);
            Debug.Log("not grounded");
        }
        // horizontal rotation (AD keys)
        Debug.DrawLine(transform.position, transform.position + hit.normal, Color.blue);
        Debug.DrawLine(transform.position, transform.position + newDirection * 20, Color.yellow);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, XZAngle, transform.rotation.eulerAngles.z), Time.deltaTime * 5f);
    }
}
