using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class handles enemy movement
// TODO add a bunch of different movement patterns

public class EnemyMovement : BaseMovement
{
    [SerializeField]private BaseShooting shootComponent;
    private Vector2 origin; // the point that this enemy is initially placed at, this is used by certain strategies
    protected BaseStrategy strategy;
    //private int moveCount;
    private int moveStep = 0;
    private int beatFracCount = 0;

    protected override void Awake()
    {
        base.Awake();
        origin = transform.position;
    }

    protected virtual void Start()
    {
        BeatManager.onBeatFrac += BeatFractionMove;
    }

    protected override void FixedUpdate()
    {
        if (!BeatManager.instance.beating) return;

        if (strategy)
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
                if (MoveAbsolute())
                    transform.Translate(move, Space.World);
                else
                    transform.Translate(move);
            }
        }
        else
            move = Vector2.zero;

        Debug.Log("moving " + move);
    }

    private void OnDestroy()
    {
        BeatManager.onBeatFrac -= BeatFractionMove;
    }

    // called every beat fraction, move to the beat!!!
    private void BeatFractionMove(int count)
    {
        if (!BeatManager.instance.beating) return;

        if (isActiveAndEnabled)
        {
            Debug.Log("beatFracCount " + beatFracCount + " moveStep " + moveStep);
            beatFracCount++;
            if (strategy)
            {
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
    }

    // called after spawning this pawn to set the strategy, should be done immediately on spawning!
    public void SetStrategy(BaseStrategy stratNew)
    {
        strategy = stratNew;
        moveStep = 0;
        beatFracCount = 0;
        if (shootComponent)
        {
            LinearStrategy strategyLinear = strategy as LinearStrategy;
            if (strategyLinear)
            {
                shootComponent.SetShooting(strategyLinear.Shoot(moveStep));
            }
        }
    }
}
