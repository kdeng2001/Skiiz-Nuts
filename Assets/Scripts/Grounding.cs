using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounding : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] int gravity = 30;
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

    private void Awake()
    {
        TryGetComponent(out rb);
    }

    private void FixedUpdate()
    {
        if(rb != null) rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        //if(IsGrounded()) { Debug.Log("is grounded"); }
        //else if(rb != null) 
        //{ 
        //    Debug.Log("not grounded");
            
        //}
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
