using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounding : MonoBehaviour
{
    Rigidbody rb;
    SphereCollider sphere;
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
        sphere = transform.parent.GetComponentInChildren<SphereCollider>();
    }

    /// <summary>
    /// returns true if player is touching ground, else returns false
    /// </summary>
    /// <returns></returns>
    public bool IsGrounded(Vector3 direction)
    {
        //Sometimes, when player is standing still, this function returns both true and false
        Debug.Log("ground direction: " + direction);
        RaycastHit hit;
        if (Physics.SphereCast(groundCheck.position, sphere.radius, direction, out hit, .5f, groundCheckLayerMask))
        {
            Debug.Log("grounded");
            //Debug.Log(Physics.OverlapBox(groundCheck.position, groundCheckDimensions, transform.rotation, groundCheckLayerMask));
            return true; 
        }
        Debug.Log("not grounded");
        return false;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(groundCheck.position + Vector3.down * .1f, sphere.radius);
    //}
}
