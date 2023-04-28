using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the controller for the player
// moves their pawn around within the set boundaries

public class PlayerMovement : BaseMovement
{
    void Update()
    {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");
    }
}
