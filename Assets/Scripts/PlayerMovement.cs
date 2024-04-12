using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] public float force = 10f;
    [SerializeField] public float maxSpeed = 10f;
    [SerializeField] public float defaultSpeed = 5f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Move(Vector2 direction)
    {
        Deccelerate(direction);
        Accelerate(direction);
        //DefaultMove();

    }
    public void Accelerate(Vector2 direction)
    {
        if (Mathf.Abs(rb.velocity.magnitude) > 10f) { return; }
        else if(PlayerActionManager.Instance.moveValue.y > 0)
        {
            rb.AddForce(1 * force * direction);
        }
    }

    public void Deccelerate(Vector2 direction)
    {
        if(rb.velocity.y == 0) { return; }
        else if(rb.velocity.y < 0f) { rb.velocity = Vector2.zero; return; }
        else if(PlayerActionManager.Instance.moveValue.y < 0)
        {
            Debug.Log("backing up");
            rb.AddForce(-1 * force * direction);
        }
    }

    //public void DefaultMove()
    //{
    //    if(PlayerActionManager.Instance.moveValue.y == 0)
    //    {
    //        if(Mathf.Abs(rb.velocity.magnitude) > defaultSpeed) 
    //        {
    //            rb.AddForce(-1 * force * direction);
    //        }
    //        if(Mathf.Abs(rb.velocity.magnitude) < defaultSpeed) 
    //        {
    //            rb.AddForce(1 * force * direction); 
    //        }
    //    }
    //}
}
