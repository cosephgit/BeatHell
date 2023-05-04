using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class handles enemy movement
// TODO add a bunch of different movement patterns

public class EnemyMovement : BaseMovement
{
    private Vector2 origin; // the point that this enemy is initially placed at, this is used by certain strategies
    private BaseStrategy strategy;
    //private int moveCount;
    private int moveStep = 0;
    private int beatFracCount = 0;

    protected override void Awake()
    {
        base.Awake();
        origin = transform.position;
    }

    private void Start()
    {
        BeatManager.onBeatFrac += BeatFractionMove;
    }

    protected override void FixedUpdate()
    {
        if (moveStep < strategy.count)
        {
            float originRotate = strategy.RotateOrigin(moveStep);

            if (originRotate != 0)
            {
                transform.RotateAround(origin, Vector3.forward, originRotate);
            }

            transform.Rotate(0, 0, strategy.TurnGradual(moveStep));
            move = strategy.Move(moveStep);
        }
        else
            move = Vector2.zero;

        if (move.magnitude > 0)
        {
            transform.Translate(move);
        }
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
            if (beatFracCount >= strategy.StepBeatFracs(moveStep))
            {
                beatFracCount = 0;
                moveStep++;

                if (moveStep >= strategy.count)
                {
                    if (strategy.loop)
                    {
                        moveStep = 0;
                    }
                    else return; // make sure not to execute anything else if we've run out of moves
                }

                // if not lerping, snap by the new turn angle
                transform.Rotate(0, 0, strategy.TurnInstant(moveStep));
            }
        }
    }

    // called after spawning this pawn to set the strategy, should be done immediately on spawning!
    public void SetStrategy(BaseStrategy stratNew)
    {
        strategy = stratNew;
    }
}
