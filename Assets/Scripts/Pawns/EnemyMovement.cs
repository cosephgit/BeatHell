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
    private BaseStrategy strategy;
    //private int moveCount;
    private int moveStep = 0;
    private int beatFracCount = 0;

    protected override void Awake()
    {
        base.Awake();
        //moveCount = Mathf.Min(moveMags.Length, moveBeatFracs.Length, moveTurns.Length, moveLerp.Length, moveShoot.Length);
        //if (Mathf.Max(moveMags.Length, moveBeatFracs.Length, moveTurns.Length, moveLerp.Length, moveShoot.Length) > moveCount)
        //{
        //    Debug.LogError(gameObject + " EnemyMovement is set up wrong, only processing the complete " + moveCount + " move pattern steps");
        //}
    }

    private void Start()
    {
        BeatManager.onBeatFrac += BeatFractionMove;
    }

    private void Update()
    {
        if (moveStep < strategy.count)
        {
            transform.Rotate(0, 0, strategy.TurnGradual(moveStep));
            move = strategy.Move(moveStep);
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
