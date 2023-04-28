using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the basic movement class for all pawns

public class BaseMovement : MonoBehaviour
{
    [SerializeField]private float speed = 1f;
    [SerializeField]private Rigidbody2D body;
    protected Vector2 move = Vector2.zero;
    private float speedTick;

    void Awake()
    {
        // store this so we don't have to calculate it every from
        speedTick = speed * Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        if (move.magnitude > 0)
        {
            Vector2 moveFrame = move * speedTick;
            transform.Translate(moveFrame);
        }
    }
}
