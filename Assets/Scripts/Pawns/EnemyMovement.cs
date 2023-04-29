using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class handles enemy movement
// TODO add a bunch of different movement patterns

public enum Pattern
{
    Line,
    Circle
}

public class EnemyMovement : BaseMovement
{
    [SerializeField]private Vector2[] moveMags; // this array contains the movement magnitudes at each step of the movement pattern
    [SerializeField]private int[] moveBeatFracs; // how many beats the matching moveDirections entry is maintained for
    [SerializeField]private float[] moveTurns; // the number of degrees to turn over moveBeats TODO implement
    [SerializeField]private bool[] moveLerp; // if true, the Mags and Turns are lerped in over moveBeats TODO implement
    [SerializeField]private bool[] moveShoot; // if true, shoot during this move TODO implement
    [SerializeField]private bool loop = true; // keep looping the pattern indefinitely
    private int moveCount;
    private int moveStep = 0;
    private int beatFracCount = 0;

    protected override void Awake()
    {
        base.Awake();
        moveCount = Mathf.Min(moveMags.Length, moveBeatFracs.Length, moveTurns.Length, moveLerp.Length, moveShoot.Length);
        if (Mathf.Max(moveMags.Length, moveBeatFracs.Length, moveTurns.Length, moveLerp.Length, moveShoot.Length) > moveCount)
        {
            Debug.LogError(gameObject + " EnemyMovement is set up wrong, only processing the complete " + moveCount + " move pattern steps");
        }
    }

    private void Start()
    {
        BeatManager.onBeatFrac += BeatFractionMove;
    }

    private void Update()
    {
        if (moveStep < moveCount)
        {
            if (moveTurns[moveStep] != 0f && moveLerp[moveStep])
            {
                float turn = moveTurns[moveStep] * Time.fixedDeltaTime;
                transform.Rotate(0, 0, turn);
            }
            move = moveMags[moveStep];
        }
        else
            move = Vector2.zero;
    }

    private void OnDestroy()
    {
        BeatManager.onBeatFrac -= BeatFractionMove;
    }

    // called every beat fraction, move to the beat!!!
    private void BeatFractionMove(int count)
    {
        if (isActiveAndEnabled)
        {
            beatFracCount++;
            if (beatFracCount >= moveBeatFracs[moveStep])
            {
                beatFracCount = 0;
                moveStep++;

                if (moveStep >= moveCount)
                {
                    if (loop)
                    {
                        moveStep = 0;
                    }
                    else return; // make sure not to execute anything else if we've run out of moves
                }

                // if not lerping, snap by the new turn angle
                if (moveTurns[moveStep] != 0f && !moveLerp[moveStep])
                {
                    transform.Rotate(0, 0, moveTurns[moveStep]);
                }
            }
        }
    }
}
