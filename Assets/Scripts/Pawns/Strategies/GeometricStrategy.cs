using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this strategy uses a combination of multiple elements to achieve smooth curved shapes, it's a lot more detailed than the basic linear strategy
// there are these elements:
// the position and rotation of the movement hub
// the position and rotation of the enemy in relation to the movement hub

public class GeometricStrategy : BaseStrategy
{
    [SerializeField]private Vector2 origin; // the origin
    [SerializeField]private Vector2[] moveMags; // this array contains the movement magnitudes at each step of the movement pattern
    [SerializeField]private int[] moveBeatFracs; // how many beat fractions this step lasts for
    [SerializeField]private float[] moveTurns; // the number of degrees to turn over moveBeats
    [SerializeField]private bool[] moveLerp; // if true, the Mags and Turns are lerped in over moveBeats
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
    public override float RotateOrigin(int step)
    {
        return 0;
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
