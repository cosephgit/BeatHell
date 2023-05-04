using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the controller for the player
// moves their pawn around within the set boundaries

public class PlayerMovement : BaseMovement
{
    [Header("Player movement bounds")]
    [SerializeField]private Rect moveBounds;

    private void Update()
    {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        transform.position = Global.Vector2Clamp(transform.position, moveBounds);
    }
}
