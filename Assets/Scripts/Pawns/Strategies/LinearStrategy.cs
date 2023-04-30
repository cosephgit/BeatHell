using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this simple strategy provides a linear movement for an enemy
// it currently has no validation, make sure all the array sizes match the count variable

public class LinearStrategy : BaseStrategy
{
    [SerializeField]private Vector2[] moveMags; // this array contains the movement magnitudes at each step of the movement pattern
    [SerializeField]private int[] moveBeatFracs; // how many beats the matching moveDirections entry is maintained for
    [SerializeField]private float[] moveTurns; // the number of degrees to turn over moveBeats TODO implement
    [SerializeField]private bool[] moveLerp; // if true, the Mags and Turns are lerped in over moveBeats TODO implement
    [SerializeField]private bool[] moveShoot; // if true, shoot during this move TODO implement

    public override float TurnInstant(int step)
    {
        if (moveLerp[step]) return 0f;
        return moveTurns[step];
    }
    public override float TurnGradual(int step)
    {
        if (moveLerp[step]) return (moveTurns[step] / (float)moveBeatFracs[step] / (float)BeatManager.beatFracFrames);
        return 0f;
    }
    public override Vector2 Move(int step)
    {
        return moveMags[step];
    }
    public override int StepBeatFracs(int step)
    {
        return moveBeatFracs[step];
    }
}
