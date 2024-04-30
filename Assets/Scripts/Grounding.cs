using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounding : MonoBehaviour
{
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

    /// <summary>
    /// returns true if player is touching ground, else returns false
    /// </summary>
    /// <returns></returns>
    public bool IsGrounded(Vector3 direction)
    {
        RaycastHit hit;
        Debug.DrawRay(groundCheck.position, direction * 1.5f);
        if (Physics.Raycast(groundCheck.position, direction, out hit, 1.5f, groundCheckLayerMask))
        {
            return true; 
        }
        return false;
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(groundCheck.position + Vector3.down * .1f, sphere.radius);
    //}
}
