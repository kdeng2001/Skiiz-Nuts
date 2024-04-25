using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerAnimation playerAnimation;
    Rigidbody rb;
    Grounding ground;
    [SerializeField] public float accelerateForce = 10f;
    [SerializeField] public float deccelerateForce = 10f;
    [SerializeField] public float maxSpeed = 10f;
    [SerializeField] public float defaultSpeed = 5f;
    private void Awake()
    {
        rb = transform.parent.GetComponentInChildren<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        ground = GetComponent<Grounding>();
    }
    public void Move(Vector3 direction)
    {
        Decelerate(direction);
        Accelerate(direction);
        Friction(direction);
        rb.AddForce(Vector3.down * ground.gravity * 5f, ForceMode.Acceleration); // gravity

    }
    public void Accelerate(Vector3 direction)
    {
        if(PlayerActionManager.Instance.moveValue.y > 0)
        {
            playerAnimation.SetAccelerate();
            Vector3 directionVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (Mathf.Abs(directionVelocity.magnitude) > maxSpeed) { return; }
            //else { rb.AddForce(1 * accelerateForce * direction, ForceMode.Acceleration); }
            //else { rb.AddTorque(1 * 10 * accelerateForce * direction, ForceMode.Acceleration); }

            //if (Mathf.Abs(rb.velocity.magnitude) > maxSpeed) { return; }
            else { rb.AddForce(1 * accelerateForce * direction, ForceMode.Acceleration); Debug.Log("Accelerate"); }
        }
    }

    public void Decelerate(Vector3 direction)
    {
        // slow to a stop
        if (PlayerActionManager.Instance.moveValue.y < 0)
        {
            // cannot go backwards
            if (rb.velocity.z == 0) { return; }
            else if (rb.velocity.z < 0f && direction.z > 0) { rb.velocity = Vector3.zero; return; }
            else if (rb.velocity.z > 0f && direction.z < 0) { rb.velocity = Vector3.zero; return; }
            Debug.Log("backing up");
            rb.AddForce(-2 * deccelerateForce * direction, ForceMode.Acceleration);
        }
    }

    public void Friction(Vector3 direction)
    {
        if (PlayerActionManager.Instance.moveValue.y == 0)
        {
            playerAnimation.SetIdle();
            //if(rb.velocity.magnitude > 0) { rb.AddForce(-1 * accelerateForce * direction /4, ForceMode.Acceleration); }
        }
    }
}
