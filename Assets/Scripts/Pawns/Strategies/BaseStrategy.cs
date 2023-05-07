using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is the base class for all types of movement pattern for enemies
// it provides a movement input Vector2 which an enemy movement class can call each frame, which is determined based on the current beat frame of the enemy
// strategies are just used as references from a prefab, so it should be impossible to change anything in the class from the outside

public class BaseStrategy : MonoBehaviour
{
    [field: SerializeField]public bool loop { get; private set; } = true;
    [field: SerializeField]public int count { get; protected set; } = 0;

    // this returns the amount of instant turning (at the start of a move transition) which the current strategy requires
    public virtual float TurnInstant(int step)
    {
        return 0f;
    }
    // returns the turning for one frame of movement if lerping is on
    public virtual float TurnGradual(int step)
    {
        return 0f;
    }
    // returns the number of degrees the enemy should rotate relative to the enemy origin in one frame
    public virtual float RotateOrigin(int step)
    {
        return 0;
    }
    // returns the x,y movement vector
    public virtual Vector2 Move(int step)
    {
        return Vector2.zero;
    }
    // returns the number of beat fractions for the current movement step
    public virtual int StepBeatFracs(int step)
    {
        return 0;
    }

    public virtual bool Shooting(int step)
    {
        return true;
    }
}
