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
        Debug.Log("XZAngle: " + XZAngle + " inputDirection: " + Mathf.RoundToInt(inputDirection));
        SetDirection(inputDirection);
        
        Debug.DrawLine(transform.position, transform.position + 10 * direction, Color.red);
    }

    public void SetDirection(float inputDirection)
    {
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (XZAngle - 90)), 0,
        Mathf.Sin(Mathf.Deg2Rad * (XZAngle + 90)));

        RaycastHit hit;
        if (Physics.Raycast(transform.position + direction, Vector3.down, out hit, Mathf.Infinity, ground.groundCheckLayerMask) && ground.IsGrounded(hit.normal * -1)) 
        {
           
            // There is still sometimes an immediate drop in velocity sometimes when hitting bumps and up/down hills
            // May have to do with whether the player is actually in contact with ground (which sometimes is not the case,
            // even when IsGrounded() returns true)
            Vector3 projected = Vector3.Project(direction, hit.normal);
            newDirection = (direction - projected).normalized;
            rb.velocity = rb.velocity.magnitude * newDirection;
            
            // z rotation for sprite uneven surfaces
            Vector2 normalPoint = Camera.main.WorldToScreenPoint(transform.position + hit.normal);
            Vector2 playerPoint = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 facePoint = Camera.main.WorldToScreenPoint(transform.position + direction);
            float angle = (Vector2.Angle(normalPoint - playerPoint, facePoint - playerPoint)); 
            if (facePoint.x > normalPoint.x)
            {
                transform.eulerAngles += Vector3.forward * angle * Time.fixedDeltaTime * .1f;
            }
            else
            {
                transform.eulerAngles += Vector3.back * angle * Time.fixedDeltaTime * .1f;
            }
        }
        else
        {
            // something here causes stuttering (when turning?)
            newDirection = direction;
            Vector3 yVelocity = Vector3.up * rb.velocity.y;
            Vector3 directionVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.velocity = directionVelocity.magnitude * direction + yVelocity;
        }
        // horizontal rotation (AD keys)
        Debug.DrawLine(transform.position, transform.position + hit.normal, Color.blue);
        Debug.DrawLine(transform.position, transform.position + newDirection * 20, Color.yellow);
        transform.eulerAngles = new(0, XZAngle, transform.eulerAngles.z);

        // gravity
        rb.AddForce(Vector3.down * ground.gravity * 5f, ForceMode.Acceleration);
    }
}
