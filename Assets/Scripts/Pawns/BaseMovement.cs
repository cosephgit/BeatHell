using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the basic movement class for all pawns
// this class must be resilient to being destroyed by another class

public class BaseMovement : MonoBehaviour
{
    [SerializeField]private float speed = 1f;
    [SerializeField]private Rigidbody2D body;
    protected Vector2 move = Vector2.zero; // a unit vector indicating the direction of movement
    private float speedTick;

    protected virtual void Awake()
    {
        // store this so we don't have to calculate it every from
        speedTick = speed * Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        if (move.magnitude > 0)
        {
            if (move.magnitude > 1f)
                move = move.normalized; // if using both axis, normalize so they can't move faster just by going diagonally

            Vector2 moveFrame = move * speedTick;
            transform.Translate(moveFrame);
        }
    }
}
