using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerAnimation playerAnimation;
    Rigidbody rb;
    Grounding ground;
    [Tooltip("The amount of forward force exerted in each fixed frame until reaching maxSpeed.")]
    [SerializeField] public float accelerateForce = 10f;
    [Tooltip("The amount of forward force exerted by default. This variable should not be modified.")]
    [SerializeField] public float defaultAccelerateForce = 10f;
    [Tooltip("The amount of backward force exerted in each fixed frame until reaching a stop.")]
    [SerializeField] public float deccelerateForce = 10f;
    [Tooltip("A multiplier that decreases the amount of force exerted when moving at speeds greater than the maxSpeed plus any bonus.")]
    [SerializeField] public float maintainForceRate = .25f;
    [Tooltip("The maximum amount of speed the player can achieve with acceleration on a flat surface.")]
    [SerializeField] public float maxSpeed = 10f;
    [Tooltip("The default maxSpeed value. This variable should not be modified.")]
    [SerializeField] public float defaultMaxSpeed = 10f;

    private float slopeBonusSpeed = 0;
    [Tooltip("A bonus multiplier to maxSpeed that allows greater speed downhill to a certain extent, depending on the angle of the slope.")]
    [SerializeField] float downSlopeBonusRate = 1.25f;
    [Tooltip("A bonus multiplier to maxSpeed that allows lesser speed uphill to a certain extent, depending on the angle of the slope.")]
    [SerializeField] float upSlopeBonusRate = 0.5f;
    private void Awake()
    {
        rb = transform.parent.GetComponentInChildren<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        ground = GetComponent<Grounding>();
    }
    public void Move(Vector3 direction)
    {
        AdjustSlopeMaxSpeed(direction);
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
            if (Mathf.Abs(directionVelocity.magnitude) > maxSpeed + slopeBonusSpeed) 
            { 
                rb.AddForce(maintainForceRate * accelerateForce * direction, ForceMode.Acceleration); 
            }
            else 
            { 
                rb.AddForce(1 * accelerateForce * direction, ForceMode.Acceleration); Debug.Log("Accelerate"); 
            }
        }
    }

    public void Decelerate(Vector3 direction)
    {
        // slow to a stop
        if (PlayerActionManager.Instance.moveValue.y < 0)
        {
            if (rb.velocity.magnitude < 1) { rb.velocity = Vector3.zero; return; }
            rb.AddForce(-1f * deccelerateForce * direction, ForceMode.Acceleration);
        }
    }

    public void Friction(Vector3 direction)
    {
        if (rb.velocity.magnitude > 0 && !(PlayerActionManager.Instance.moveValue.y < 0)) 
        { 
            rb.AddForce(-1 * accelerateForce * direction * .1f * rb.velocity.magnitude * .1f, ForceMode.Acceleration); 
        }
        if (PlayerActionManager.Instance.moveValue.y == 0)
        {
            playerAnimation.SetIdle();
            
        }
    }

    private void AdjustSlopeMaxSpeed(Vector3 direction) 
    {
        slopeBonusSpeed = direction.y * -1 * 10;
        //if (direction.y < 0) { slopeBonusSpeed *= downSlopeBonusRate; }
        //else { slopeBonusSpeed *= upSlopeBonusRate; }
        Debug.Log("slopeBonusSpeed" + (slopeBonusSpeed + maxSpeed));
    }
}
