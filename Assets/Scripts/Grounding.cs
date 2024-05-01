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
    /// Length of ray that checks for ground
    /// </summary>
    [SerializeField] public float rayLength = 1.5f;
    /// <summary>
    /// returns true if player is touching ground, else returns false
    /// </summary>
    public bool IsGrounded(Vector3 direction)
    {
        RaycastHit hit;
        Debug.DrawRay(groundCheck.position, direction * rayLength);
        if (Physics.Raycast(groundCheck.position, direction, out hit, rayLength, groundCheckLayerMask))
        {
            return true; 
        }
        return false;
    }
}
