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
    public virtual float TurnGradual(int step)
    {
        return 0f;
    }
    public virtual Vector2 Move(int step)
    {
        return Vector2.zero;
    }
    public virtual int StepBeatFracs(int step)
    {
        return 0;
    }
}
