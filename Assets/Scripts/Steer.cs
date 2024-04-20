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
    SpriteRenderer sprite;
    private void Awake()
    {
        rb = transform.parent.GetComponentInChildren<Rigidbody>();
        XZAngle = (int) transform.rotation.eulerAngles.y;
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (XZAngle - 90)), 0,
        Mathf.Sin(Mathf.Deg2Rad * (XZAngle + 90)));
        steerRate = baseSteerRate;
        ground = GetComponent<Grounding>();
        sprite = GetComponent<SpriteRenderer>();
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
        SetDirection();
        
        Debug.DrawLine(transform.position, transform.position + 10 * direction, Color.red);
    }

    public void SetDirection()
    {
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (XZAngle - 90)), 0,
        Mathf.Sin(Mathf.Deg2Rad * (XZAngle + 90)));

        RaycastHit hit;
        //Debug.Log(direction);
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, ground.groundCheckLayerMask);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, ground.groundCheckLayerMask) && ground.IsGrounded()) 
        {
            // Note: not sure why, but normalizing newDirection instead of hit.normal resolved issue of slowing down going up/down slopes
            Vector3 projected = Vector3.Project(direction, hit.normal);
            newDirection = (direction - projected).normalized;
            rb.velocity = rb.velocity.magnitude * newDirection;
            //Debug.DrawLine(transform.position, transform.position + newDirection * 20, Color.yellow);
            Debug.Log("grounded");
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            if(angle < 50)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, angle), Time.deltaTime * .75f);

            }
            //Debug.Log("angle: " + angle);
            //Debug.Log("normal ground vector: " + hit.normal);
            //Debug.DrawLine(transform.position, transform.position + hit.normal * 20, Color.blue);
            //Debug.DrawLine(transform.position, transform.position + Vector3.up * 20, Color.cyan);
        }
        else
        {
            newDirection = direction;
            Vector3 yVelocity = Vector3.up * rb.velocity.y;
            Vector3 directionVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.velocity = directionVelocity.magnitude * direction + yVelocity;
            rb.AddForce(Vector3.down * ground.gravity, ForceMode.Acceleration);
            Debug.Log("not grounded");
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, XZAngle, transform.rotation.eulerAngles.z), Time.deltaTime * 5);
    }
}
