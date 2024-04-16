using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerAnimation playerAnimation;
    Rigidbody rb;
    [SerializeField] public float accelerateForce = 10f;
    [SerializeField] public float deccelerateForce = 10f;
    [SerializeField] public float maxSpeed = 10f;
    [SerializeField] public float defaultSpeed = 5f;
    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }
    public void Move(Vector3 direction)
    {
        Deccelerate(direction);
        Accelerate(direction);
        DefaultMove(direction);

    }
    public void Accelerate(Vector3 direction)
    {
        if(PlayerActionManager.Instance.moveValue.y > 0)
        {
            playerAnimation.SetAccelerate();
            Vector3 directionVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (Mathf.Abs(directionVelocity.magnitude) > maxSpeed) { return; }
            else { rb.AddForce(1 * accelerateForce * direction); }
        }
    }

    public void Deccelerate(Vector3 direction)
    {
        // slow to a stop
        if(PlayerActionManager.Instance.moveValue.y < 0)
        {
            // cannot go backwards
            if (rb.velocity.z == 0) { return; }
            else if (rb.velocity.z < 0f && direction.z > 0) { rb.velocity = Vector3.zero; return; }
            else if (rb.velocity.z > 0f && direction.z < 0) { rb.velocity = Vector3.zero; return; }
            Debug.Log("backing up");
            rb.AddForce(-1 * deccelerateForce * direction);
        }
    }

    public void DefaultMove(Vector3 direction)
    {
        if (PlayerActionManager.Instance.moveValue.y == 0)
        {
            playerAnimation.SetIdle();
            //if (Mathf.Abs(rb.velocity.magnitude) > defaultSpeed)
            //{
            //    rb.AddForce(-1 * accelerateForce * direction);
            //}
            //if (Mathf.Abs(rb.velocity.magnitude) < defaultSpeed)
            //{
            //    rb.AddForce(1 * accelerateForce * direction);
            //}
        }
    }
}
