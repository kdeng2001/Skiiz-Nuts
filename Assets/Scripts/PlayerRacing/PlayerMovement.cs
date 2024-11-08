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
    /// <summary>
    /// Calls functions that check player input and determines movement behavior
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector3 direction)
    {
        AdjustSlopeMaxSpeed(direction);
        Decelerate(direction);
        Accelerate(direction);
        Friction(direction); // bonus friction in addition to material
        rb.AddForce(5f * ground.gravity * Vector3.down, ForceMode.Acceleration); // gravity

    }
    /// <summary>
    /// Speeds the player up to maxSpeed + slopeBonusSpeed
    /// </summary>
    /// <param name="direction">the direction the player is facing/moving currently</param>
    private void Accelerate(Vector3 direction)
    {
        if(PlayerActionManager.Instance.moveValue.y > 0)
        {
            playerAnimation.SetAccelerate();
            Vector3 directionVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            // Uses maintainForce, which should be smaller than accelerateForce, to allow player to maintain some speed when over maxSpeed + slopeBonusSpeed
            if (Mathf.Abs(directionVelocity.magnitude) > maxSpeed + slopeBonusSpeed) 
            { 
                rb.AddForce(maintainForceRate * accelerateForce * direction, ForceMode.Acceleration); 
            }
            // Uses accelerateForce value to push player in the direction
            else 
            { 
                rb.AddForce(1 * accelerateForce * direction, ForceMode.Acceleration); 
            }
        }
    }
    /// <summary>
    /// Slows the player to an eventual stop using deccelerateForce
    /// </summary>
    /// <param name="direction">the direction the player is moving currently</param>
    private void Decelerate(Vector3 direction)
    {
        // slow to a stop
        if (PlayerActionManager.Instance.moveValue.y < 0)
        {
            if (rb.velocity.magnitude < 1) { rb.velocity = Vector3.zero; return; }
            rb.AddForce(-1f * deccelerateForce * direction, ForceMode.Acceleration);
        }
    }
    /// <summary>
    /// Applies additional friction to the player, slowing the player when moving
    /// </summary>
    /// <param name="direction"></param>
    private void Friction(Vector3 direction)
    {
        if (rb.velocity.magnitude > 0 && !(PlayerActionManager.Instance.moveValue.y < 0)) 
        { 
            rb.AddForce(.1f * .1f * -1 * accelerateForce * rb.velocity.magnitude * direction, ForceMode.Acceleration); 
        }
        if (PlayerActionManager.Instance.moveValue.y == 0) { playerAnimation.SetIdle(); }
    }
    /// <summary>
    /// Adjusts the player's max speed, to allow moving faster down, and slower up
    /// </summary>
    /// <param name="direction">This should be the direction the player is moving in</param>
    private void AdjustSlopeMaxSpeed(Vector3 direction) 
    {
        slopeBonusSpeed = direction.y * -1 * 10;
        if (direction.y < 0) { slopeBonusSpeed *= downSlopeBonusRate; }
        else { slopeBonusSpeed *= upSlopeBonusRate; }
    }
}
