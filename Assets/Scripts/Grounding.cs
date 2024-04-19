using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounding : MonoBehaviour
{
    Rigidbody rb;
    Steer steer;
    [SerializeField] public int gravity = 30;
    /// <summary>
    /// Transform under player
    /// </summary>
    [SerializeField] public Transform groundCheck;
    /// <summary>
    /// radius of ground check
    /// </summary>
    [SerializeField] public Vector3 groundCheckDimensions = new Vector3(1, 1, 1);
    /// <summary>
    /// layers that are considered "ground"
    /// </summary>
    [SerializeField] public LayerMask groundCheckLayerMask;
    private RaycastHit hit;
    Vector3 newDirection;

    private void Awake()
    {
        //TryGetComponent(out rb);
        rb = transform.parent.GetComponentInChildren<Rigidbody>();
        steer = GetComponent<Steer>();
    }

    private void FixedUpdate()
    {
        //if(rb != null) rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        //if(IsGrounded()) { Debug.Log("is grounded"); }
        //else if(rb != null) 
        //{ 
        //    Debug.Log("not grounded");

        //}
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundCheckLayerMask);
        Debug.DrawLine(transform.position, transform.position + hit.normal * 10, Color.blue);
        Debug.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, 90) * hit.normal * 10, Color.green);
        //if (steer.direction.z < 0) { newDirection = new Vector3(steer.direction.x, -1 * (Quaternion.Euler(0, 0,90) * hit.normal).y, steer.direction.z); }
        //else { newDirection = new Vector3(steer.direction.x, (Quaternion.Euler( 0, 0,90) * hit.normal).y, steer.direction.z); }


        //if(Mathf.Abs(steer.direction.z) > 0.5f) 
        //{
        //    if (steer.direction.z > 0 && steer.direction.x >= 0) { newDirection = new Vector3(steer.direction.x, (Quaternion.Euler(90, 0, 0) * hit.normal).y, steer.direction.z); Debug.Log("+z, +x"); }
        //    else if(steer.direction.z > 0 && steer.direction.x <= 0) { newDirection = new Vector3(steer.direction.x, (Quaternion.Euler(90, 0, 0) * hit.normal).y, steer.direction.z); Debug.Log("+z, -x"); }
        //    else if(steer.direction.z < 0 && steer.direction.x >= 0) { newDirection = new Vector3(steer.direction.x, -1 * (Quaternion.Euler(90, 0, 0) * hit.normal).y, steer.direction.z); Debug.Log("-z, +x"); }
        //    else if(steer.direction.z < 0 && steer.direction.x <= 0) { newDirection = new Vector3(steer.direction.x, -1 * (Quaternion.Euler(90, 0, 0) * hit.normal).y, steer.direction.z); Debug.Log("-z, -x"); }
        //}
        //else
        //{
        //    if (steer.direction.z > 0 && steer.direction.x >= 0) { newDirection = new Vector3(steer.direction.x, -1 * (Quaternion.Euler(0, 0, 90) * hit.normal).y, steer.direction.z); Debug.Log("+z, +x"); }
        //    else if (steer.direction.z > 0 && steer.direction.x <= 0) { newDirection = new Vector3(steer.direction.x, (Quaternion.Euler( 0, 0, 90) * hit.normal).y, steer.direction.z); Debug.Log("+z, -x"); }
        //    else if (steer.direction.z < 0 && steer.direction.x >= 0) { newDirection = new Vector3(steer.direction.x, -1 * (Quaternion.Euler(0, 0, 90) * hit.normal).y, steer.direction.z); Debug.Log("-z, +x"); }
        //    else if (steer.direction.z < 0 && steer.direction.x <= 0) { newDirection = new Vector3(steer.direction.x, (Quaternion.Euler(0, 0, 90) * hit.normal).y, steer.direction.z); Debug.Log("-z, -x"); }
        //}

  
        
        
        
        
        //Debug.DrawLine(transform.position, transform.position + newDirection * 10, Color.yellow);
        //Debug.Log("Direction: "+ newDirection);
    }

    /// <summary>
    /// returns true if player is touching ground, else returns false
    /// </summary>
    /// <returns></returns>
    public bool IsGrounded()
    {
        if (Physics.OverlapBox (groundCheck.position, groundCheckDimensions, transform.rotation, groundCheckLayerMask).Length == 0) 
        {
            //Debug.Log(Physics.OverlapBox(groundCheck.position, groundCheckDimensions, transform.rotation, groundCheckLayerMask));
            return false; 
        }
        //Debug.Log(Physics.OverlapBox(groundCheck.position, groundCheckDimensions, transform.rotation, groundCheckLayerMask));
        return true ;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, groundCheckDimensions);
    }
}
